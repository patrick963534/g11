using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Octopus.Net
{
    internal class OutgoingPackagePool
    {
        private static object m_lockobject = new object();
        private static OutgoingPackagePool s_singleton;

        private List<NetPackage> m_unprocessed = new List<NetPackage>();
        private Dictionary<int, NetPackage> m_processed = new Dictionary<int, NetPackage>();

        static OutgoingPackagePool()
        {
            s_singleton = new OutgoingPackagePool();
        }

        public static void Add(NetPackage package)
        {
            s_singleton.AddToTail(package);
        }

        public static void AddFirst(NetPackage package)
        {
            s_singleton.AddToFirst(package);
        }

        public static NetPackage PeekFirst()
        {
            return s_singleton.Dequeue();
        }

        public static void RemoveProcessedPackage(int packageID)
        {
            s_singleton.RemoveProcessed(packageID);
        }

        private void AddToTail(NetPackage package)
        {
            if (package.RemoteEP == null)
                Debugger.Break();

            lock (m_lockobject)
            {
                s_singleton.m_unprocessed.Add(package);
            }
        }

        private void AddToFirst(NetPackage package)
        {
            if (package.RemoteEP == null)
                Debugger.Break();

            lock (m_lockobject)
            {
                s_singleton.m_unprocessed.Insert(0, package);
            }
        }

        private void RemoveProcessed(int packageID)
        {
            lock (m_lockobject)
            {
                if (s_singleton.m_processed.ContainsKey(packageID))
                    s_singleton.m_processed.Remove(packageID);
            }  
        }

        private NetPackage Dequeue()
        {
            Reorganize();

            lock (m_lockobject)
            {
                if (s_singleton.m_unprocessed.Count == 0)
                    return null;

                NetPackage package = s_singleton.m_unprocessed[0];
                s_singleton.m_unprocessed.RemoveAt(0);
                s_singleton.m_processed.Add(package.ID, package);

                return package;
            }
        }

        private void Reorganize()
        {
            if (m_unprocessed.Count > 100)
            {
                lock (m_lockobject)
                {
                    foreach (NetPackage pkg in m_processed.Values)
                    {
                        m_unprocessed.Add(pkg);
                    }

                    m_processed.Clear();
                }
            }
        }
    }
}
