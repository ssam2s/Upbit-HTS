using System.Windows.Forms;

namespace GUI_FINAL_PROJECT
{
    public partial class wait : Form
    {
        public wait()
        {
            InitializeComponent();
        }
        public void UpdateBar(int num)
        {
            if (num == 0)
            {
                this.progressBar1.Value = 100;
                this.Close();
            }

            if (this.progressBar1.Value == 100)
            {
                this.Close();
            }
            else
            {
                this.progressBar1.Value += num;
            }
        }
    }
}
