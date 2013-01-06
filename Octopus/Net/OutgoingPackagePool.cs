using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using Octopus.Commands;
using Octopus.Core;

namespace Octopus.Net
{
    public class OutgoingPackagePool
    {
        private static OutgoingPackagePool s_singleton;

        private static object m_lockobject = new object();
        private static List<int> m_removedIDs = new List<int>();

        private List<NetPackage> m_unprocessed = new List<NetPackage>();
        private Dictionary<int, PackageLife> m_processed = new Dictionary<int, PackageLife>();

        static OutgoingPackagePool()
        {
            s_singleton = new OutgoingPackagePool();
        }

        public static void Add(NetPackage[] packages)
        {
            lock (m_lockobject)
            {
                s_singleton.m_unprocessed.AddRange(packages);
            }
        }

        public static void AddFirst(NetPackage[] packages)
        {
            lock (m_lockobject)
            {
                s_singleton.m_unprocessed.InsertRange(0, packages);
            }
        }

        public static int GetUnprocessedPackageCount()
        {
            lock (m_lockobject)
            {
                return s_singleton.m_unprocessed.Count;
            }
        }

        public static int GetProcessedPackageInPoolCount()
        {
            lock (m_lockobject)
            {
                return s_singleton.m_processed.Count;
            }
        }

        public static void RemoveProcessedPackage(int packageID)
        {
            lock (m_lockobject)
            {
                Logger.WriteLine(string.Format("Remove package with ID: {0}", packageID));
                s_singleton.m_processed.Remove(packageID);
            }  
        }

        public static void GrabProcessPackages(int ellapse, List<NetPackage> pkgs, int expected_count)
        {
            lock (m_lockobject)
            {
                int i = 0;
                foreach (PackageLife pkg in s_singleton.m_processed.Values)
                {
                    if (pkg.Update(ellapse))
                    {
                        pkgs.Add(pkg.NetPackage);
                        i++;
                    }

                    if (i >= expected_count)
                        break;
                }
            }
        }

        public static void DequeueUnprocess(List<NetPackage> pkgs)
        {
            lock (m_lockobject)
            {
                int sz = 0;
                foreach (NetPackage pkg in pkgs)
                {
                    sz += pkg.Buffer.Length;
                }

                while (s_singleton.m_unprocessed.Count != 0)
                {
                    NetPackage pkg = s_singleton.m_unprocessed[0];

                    sz += pkg.Buffer.Length;
                    if (sz >= NetService.SocketBufferSize)
                        break;


                    s_singleton.m_unprocessed.RemoveAt(0);

                    if (!pkg.IsRemoveProcessedPackageType)
                        s_singleton.m_processed.Add(pkg.ID, new PackageLife(pkg));

                    pkgs.Add(pkg);
                }                
            }
        }

        public static void RemoveDeadProcessed()
        {
            lock (m_lockobject)
            {
                if (s_singleton.m_processed.Count != 0)
                {
                    m_removedIDs.Clear();

                    foreach (PackageLife pkg in s_singleton.m_processed.Values)
                    {
                        if (pkg.IsDead)
                        {
                            Logger.Counter_Dead_Pkg();

                            if (pkg.NetPackage.OrderID == 1)
                                Logger.WriteLine(string.Format("Package with command '{0}' is dead.", (NetCommandType)pkg.NetPackage.CommandID));
                            else
                                Logger.WriteLine(string.Format("Part package with command '{0}' is dead.", (NetCommandType)pkg.NetPackage.CommandID));

                            m_removedIDs.Add(pkg.NetPackage.ID);
                        }
                    }

                    foreach (int id in m_removedIDs)
                    {
                        s_singleton.m_processed.Remove(id);
                    }
                }                
            }            
        }

        private class PackageLife
        {
            private static List<int> s_maxTimes;

            private NetPackage m_pkg;
            private int m_resendCounter;
            private int m_timer;

            public PackageLife(NetPackage pkg)
            {
                m_pkg = pkg;
                m_resendCounter = 20;
                m_timer = 0;

                if (s_maxTimes == null)
                {
                    s_maxTimes = new List<int>();
                    for (int i = 0; i < m_resendCounter; i++)
                    {
                        s_maxTimes.Add(200 + 100 * (m_resendCounter - i - 1));
                    }
                }
            }

            public NetPackage NetPackage
            {
                get { return m_pkg; }
            }

            public bool IsDead
            {
                get { return m_resendCounter <= 0; }
            }

            public bool Update(int ellapse)
            {
                m_timer -= ellapse;

                if (m_timer <= 0 && m_resendCounter > 0)
                {
                    m_resendCounter--;

                    if (m_resendCounter >= 0 && m_resendCounter < s_maxTimes.Count)
                        m_timer = s_maxTimes[m_resendCounter];
                    else
                        m_timer = 300;

                    return true;
                }

                return false;
            }
        }
    }
}
