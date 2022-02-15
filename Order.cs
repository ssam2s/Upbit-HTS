using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI_FINAL_PROJECT
{
    public partial class Order : Form
    {
        // API Authorization
        UpbitAPI U = new UpbitAPI("4BXi1ICjjOjwE2HJCh4SgLYOtEg9JhwDxGF6Mtju", "Npn69gbWwxTYud79GYYQSP5STfJxHy2W02OV80hJ");

        public Order()
        {
            InitializeComponent();
        }

        public void init(string name, string stat, string mode, string amount, string price, string total)
        {
            if (mode == "매수")
            {
                check_title.Text = "매수 주문 확인";
                check_title.ForeColor = Color.Red;
                check_name.Text = name;
                check_order_stat.Text = stat;
                check_buy_sell.Text = mode;
                check_buy_sell.ForeColor = Color.Red;
                check_amount.Text = amount + " " + name;
                check_price.Text = price + " KRW";
                check_price_total.Text = total;
            }
            else if (mode == "매도")
            {
                check_title.Text = "매도 주문 확인";
                check_title.ForeColor = Color.Blue;
                check_name.Text = name;
                check_order_stat.Text = stat;
                check_buy_sell.Text = mode;
                check_buy_sell.ForeColor = Color.Blue;
                check_amount.Text = amount + " " + name;
                check_price.Text = price + " KRW";
                check_price_total.Text = total;
            }
            else
            {
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string market_name = "KRW-" + check_name.Text;
            decimal volume; 
            decimal price;
            
            if (check_title.Text.StartsWith("매수"))
            {
                volume = Convert.ToDecimal(check_amount.Text.Replace(check_name.Text, ""));
                price = Convert.ToDecimal(check_price.Text.Replace("KRW", "").Replace(",", ""));
                volume = Convert.ToDecimal(Convert.ToDouble(volume) * 0.995);
                UpbitAPI.UpbitOrderSide ordertype = UpbitAPI.UpbitOrderSide.bid;
                UpbitAPI.UpbitOrderType ordertype_ = UpbitAPI.UpbitOrderType.limit;
                U.MakeOrder(market_name, ordertype, volume, price, ordertype_);
                this.Close();
                MessageBox.Show("주문 등록 완료");
            }
            else if (check_title.Text.StartsWith("매도"))
            {
                volume = Convert.ToDecimal(check_amount.Text.Replace(check_name.Text, ""));
                price = Convert.ToDecimal(check_price.Text.Replace("KRW", ""));
                UpbitAPI.UpbitOrderSide ordertype = UpbitAPI.UpbitOrderSide.ask;
                UpbitAPI.UpbitOrderType ordertype_ = UpbitAPI.UpbitOrderType.limit;
                U.MakeOrder(market_name, ordertype, volume, price, ordertype_);
                this.Close();
                MessageBox.Show("주문 등록 완료");
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
            MessageBox.Show("주문 등록 취소");
        }
    }
}
