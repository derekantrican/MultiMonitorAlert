using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MultiMonitorAlert
{
    public partial class Dialog : Form
    {
        List<Dialog> dialogList = new List<Dialog>();
        public Dialog(string message, bool isParent = false)
        {
            InitializeComponent();

            labelMessage.Text = message;
            ResizeFormToMessage();

            if (isParent)
            {
                Screen parentDialogScreen = Screen.FromControl(this);

                foreach (Screen screen in Screen.AllScreens)
                {
                    if (screen.Equals(parentDialogScreen))
                        continue; //The current window is already being shown on this screen

                    Dialog dialog = new Dialog(message);
                    dialog.Dismiss += Dialog_Dismiss;
                    dialogList.Add(dialog);
                    dialog.Show();
                    MoveAndCenterToScreen(screen, dialog);
                }
            }
        }

        private void ResizeFormToMessage()
        {
            int titleBarHeight = (RectangleToScreen(this.ClientRectangle)).Top - this.Top;

            int calcHeight = titleBarHeight + flowLayoutPanel1.Height + labelMessage.Height;
            int calcWidth = buttonDismiss.Width;

            int minHeight = 150;
            int minWidth = 300;

            this.Height = minHeight > calcHeight ? minHeight : calcHeight;
            this.Width = minWidth > calcWidth ? minWidth : calcWidth;
        }

        public void MoveAndCenterToScreen(Screen targetScreen, Dialog dialogToMove)
        {
            Rectangle workingArea = targetScreen.WorkingArea;
            dialogToMove.Location = new Point()
            {
                X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - this.Width) / 2),
                Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - this.Height) / 2)
            };
        }

        private void Dialog_Dismiss()
        {
            foreach (Dialog dialog in dialogList)
                dialog.Close();

            this.Close();
        }

        private void buttonDismiss_Click(object sender, EventArgs e)
        {
            this.Close();
            Dismiss?.Invoke();
        }

        public delegate void DismissDelegate();
        public event DismissDelegate Dismiss;

        private void Dialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dismiss?.Invoke();
        }
    }
}
