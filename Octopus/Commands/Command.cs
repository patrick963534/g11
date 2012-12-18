﻿using System;
using System.Collections.Generic;
using System.Text;
using Octopus.Net;
using Octopus.Core;
using System.Net;

namespace Octopus.Commands
{
    // Tell the receiver what to do. :D
    public enum NetCommandType
    {
        RemoveProcessedPackage = 0x200,
        AppendTextMessage,
        BroadcastFindUser,
        AddUser,
    }

    internal abstract class Cmd
    {
        internal void Execute()
        {
            ExecuteImpl();
        }

        protected abstract void ExecuteImpl();
    }

    internal class NP_RemoveProcessedPackageCmd : Cmd
    {
        private int m_packageID;

        public NP_RemoveProcessedPackageCmd(int packageID)
        {
            m_packageID = packageID;
        }

        protected override void ExecuteImpl()
        {
            OutgoingPackagePool.RemoveProcessedPackage(m_packageID);
        }
    }

    internal class NP_AppendTextMessageCmd : Cmd
    {
        private string m_text;
        private UserInfo m_user;

        public NP_AppendTextMessageCmd(byte[] data, UserInfo user)
        {
            m_text = Encoding.UTF8.GetString(data);
            m_user = user;
        }

        protected override void ExecuteImpl()
        {
            m_user.AppendMessage(m_text);
            Workbench.Log("Add message:" + m_text);
            if (m_user.Chatter == null)
                Workbench.Log("Chatter is null.");
        }
    }

    internal class NP_BroadcastFindUserCmd : Cmd
    {
        private string m_hostname;
        private IPEndPoint m_remoteIP;

        public NP_BroadcastFindUserCmd(byte[] data, IPEndPoint remoteIP)
        {
            m_remoteIP = new IPEndPoint(remoteIP.Address, remoteIP.Port);
            m_hostname = Encoding.UTF8.GetString(data);
        }

        protected override void ExecuteImpl()
        {
            OutgoingPackagePool.AddFirst(NetPackageGenerater.AddUser(m_remoteIP));
            Workbench.AddClient(m_hostname, m_remoteIP);
        }
    }

    internal class NP_AddUserCmd : Cmd
    {
        private IPEndPoint m_remoteIP;
        private string m_hostname;

        public NP_AddUserCmd(byte[] data, IPEndPoint remoteIP)
        {
            m_remoteIP = new IPEndPoint(remoteIP.Address, remoteIP.Port);
            m_hostname = Encoding.UTF8.GetString(data);
        }

        protected override void ExecuteImpl()
        {
            Workbench.AddClient(m_hostname, m_remoteIP);
        }
    }
}