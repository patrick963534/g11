using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Octopus.Core;

namespace Octopus.Controls.ChatControls
{
    public partial class MsgRichViewer : UserControl
    {
        private int m_msgIndex;

        public MsgRichViewer()
        {
            InitializeComponent();
            
            m_msgViewer.Navigate("about:blank");
            m_msgViewer.Document.Write("<html><body></body></html>");
        }

        public void UpdateMessages(MessageStore store)
        {
            while (m_msgIndex < store.MsgCount)
            {
                HtmlElement he = m_msgViewer.Document.CreateElement(string.Empty);
                he.InnerHtml = store.GetFormmattedMsg(m_msgIndex);
                m_msgViewer.Document.Body.AppendChild(he);
                m_msgIndex++;
            }

            m_msgViewer.Document.Window.ScrollTo(0, Int16.MaxValue);
        }
    }
}
