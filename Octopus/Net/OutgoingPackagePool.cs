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

        public static void Add(NetPackage package)
        {
            lock (m_lockobject)
            {
                s_singleton.m_unprocessed.Add(package);
            }
        }

        public static void AddFirst(NetPackage package)
        {
            lock (m_lockobject)
            {
                s_singleton.m_unprocessed.Insert(0, package);
            }
        }

        public static void RemoveProcessedPackage(int packageID)
        {
            lock (m_lockobject)
            {
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
                        if (i >= expected_count)
                        {
                            pkgs.Add(pkg.NetPackage);
                            i++;
                        }                        
                    }
                }
            }
        }

        public static void DequeueUnprocess(List<NetPackage> pkgs, int expected_count)
        {
            lock (m_lockobject)
            {
                int i = 0;
                while (s_singleton.m_unprocessed.Count != 0)
                {
                    NetPackage pkg = s_singleton.m_unprocessed[0];
                    s_singleton.m_unprocessed.RemoveAt(0);

                    if (!pkg.IsRemoveProcessedPackageType)
                        s_singleton.m_processed.Add(pkg.ID, new PackageLife(pkg));

                    pkgs.Add(pkg);

                    if (++i >= expected_count)
                        break;
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
                            if (pkg.NetPackage.OrderID == 1)
                            {
                                Logger.WriteLine(string.Format("Package with command '{0}' is dead.", (NetCommandType)pkg.NetPackage.CommandID));
                            }

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
                m_resendCounter = 10;
                m_timer = 0;

                if (s_maxTimes == null)
                {
                    s_maxTimes = new List<int>();
                    for (int i = 0; i < m_resendCounter; i++)
                    {
                        s_maxTimes.Add(300 + 600 * (m_resendCounter - i - 1));
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
