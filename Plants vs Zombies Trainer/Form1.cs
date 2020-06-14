using MemoryHack;
using System;
using System.Windows.Forms;

namespace Plants_vs_Zombies_Trainer
{
    public partial class Form1 : Form
    {
        MyMemory memory;
        private const int autoAddress = 0x004352F2;
        private const int fastRechargeAddress = 0x004958C5;
        private const int nomalZombieAddress = 0x0054626C;
        private const int hatZombieAddress = 0x00545B36;
        private const int shieldZombieAddress = 0x00545781;

        private const int AUTO = 0xEB;
        private const int NOTAUTO = 0x75;
        private const int FASTRECHARGE = 0x9090;
        private const int NOTFASTRECHARGE = 0x147E;
        private const int ONEHIT = 0x9090;
        private const int NOMALNOTONEHIT = 0x1D7F;
        private const int HEADNOTONEHIT = 0x1175;
        private const int GUARDNOTONEHIT = 0x1875;
        private int baseAddr;
        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            memory = new MyMemory("popcapgame1");
            if (memory.isOK())
            {
                int auto = memory.ReadByte(autoAddress);
                if (auto == AUTO)
                {
                    cbAtuto.Checked = true;
                }

                int fast = memory.ReadUShort(fastRechargeAddress);
                if (fast == FASTRECHARGE)
                {
                    cbTime.Checked = true;
                }
                int onehit = memory.ReadUShort(nomalZombieAddress);
                if (onehit == ONEHIT)
                {
                    cbOnehit.Checked = true;
                }
                button3.Visible = false; // sorry vì có mấy nút mình lười không đặt lại tên, nó là nút load game
                baseAddr = memory.GetBaseAddress();
                //MessageBox.Show("Load game oke");
            }
            else
            {
                MessageBox.Show("mở game lên trước");
                //Environment.Exit(1);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (GameisRunning())
            {
                if (cbAtuto.Checked)
                {
                    memory.WriteNumber(autoAddress, AUTO, 1);
                }
                else
                {
                    memory.WriteNumber(autoAddress, NOTAUTO, 1);
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (GameisRunning())
            {
                int value = 0;
                int[] sunOffset = { 0x0032F3F4 + baseAddr, 0x68, 0x320, 0x18, 0x4, 0x4, 0x8, 0x5578 };
                for (int i = 0; i < sunOffset.Length - 1; i++)
                {
                    value = memory.ReadInt(sunOffset[i] + value);
                }
                int addr = value + sunOffset[sunOffset.Length - 1];
                value = memory.ReadInt(addr);
                bool isnumber = int.TryParse(textBox1.Text, out int num);
                if (isnumber)
                {
                    memory.WriteNumber(addr, value + num, 4);
                }
            }

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (GameisRunning())
            {
                if (checkBox1.Checked)
                {
                    this.TopMost = true;
                }
                else
                {
                    this.TopMost = false;
                }
            }

        }

        private bool GameisRunning()
        {
            if (memory.isOK())
            {
                return true;
            }
            else
            {
                MessageBox.Show("Game đã bị tắt");
                button3.Visible = true;
                return false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (GameisRunning())
            {
                int value = 0;
                int[] coinOffset = { 0x0032E77C + baseAddr, 0x18, 0x10, 0x14, 0x10, 0x4, 0x4, 0x84 };
                for (int i = 0; i < coinOffset.Length - 1; i++)
                {
                    value = memory.ReadInt(coinOffset[i] + value);
                }
                int addr = value + coinOffset[coinOffset.Length - 1];
                value = memory.ReadInt(addr);
                bool isnumber = int.TryParse(textBox1.Text, out int num);
                if (isnumber)
                {
                    memory.WriteNumber(addr, value + num / 10, 4);

                }
            }
        }

        private void cbTime_CheckedChanged(object sender, EventArgs e)
        {
            if (GameisRunning())
            {
                if (cbTime.Checked)
                {
                    memory.WriteNumber(fastRechargeAddress, FASTRECHARGE, 2);
                }
                else
                {
                    memory.WriteNumber(fastRechargeAddress, NOTFASTRECHARGE, 2);
                }
            }
        }

        private void cbOnehit_CheckedChanged(object sender, EventArgs e)
        {
            if (GameisRunning())
            {
                if (cbOnehit.Checked)
                {
                    memory.WriteNumber(nomalZombieAddress, ONEHIT, 2);
                    memory.WriteNumber(hatZombieAddress, ONEHIT, 2);
                    memory.WriteNumber(shieldZombieAddress, ONEHIT, 2);
                }
                else
                {
                    memory.WriteNumber(nomalZombieAddress, NOMALNOTONEHIT, 2);
                    memory.WriteNumber(hatZombieAddress, HEADNOTONEHIT, 2);
                    memory.WriteNumber(shieldZombieAddress, GUARDNOTONEHIT, 2);
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://uongsuadaubung.blogspot.com/");
        }
    }
}
