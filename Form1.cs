using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using CoolSms;

namespace GUI_FINAL_PROJECT
{
    public partial class Form1 : Form
    {
        // Upbit API Authorization
        UpbitAPI U = new UpbitAPI("4BXi1ICjjOjwE2HJCh4SgLYOtEg9JhwDxGF6Mtju", "Npn69gbWwxTYud79GYYQSP5STfJxHy2W02OV80hJ");

        // Sms API Authorization
        SmsApi SMS = new SmsApi(new SmsApiOptions
        {
            ApiKey = "NCS1QAKUOH63CMDL",
            ApiSecret = "SDGSBXVY8ZONIMFSGKNGFGL7XVRHPSY5",
            DefaultSenderId = "01098025053"
        });

        public int IsMarketLoaded = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region 프로그램 초기 실행
            // 시작 탭 페이지 설정
            tabControl1.SelectedIndex = 1;
            tabControl2.SelectedIndex = 0;
            #endregion
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region 탭 페이지 변경
            // 탭 페이지 구분을 위한 인덱스 정의
            int index = tabControl1.SelectedIndex;

            if (index == 0) // 시세 조회 및 주문 탭
            {
                Load_TradeTab();
            }
            else if (index == 1)    // 투자 내역 탭
            {
                Load_InvestTab();
            }
            else if (index == 2)    // 관심 종목 탭
            {
                Load_FavoriteTab();
            }
            #endregion
        }

        public void Load_TradeTab()
        {
            #region 시세 주문 탭 로드

            // 최초 로드 했다면 재로딩 필요 X
            if (IsMarketLoaded == 1)
            {
                return;
            }
            
            // 로딩화면 띄우기
            wait s = new wait();
            s.Show();

            // 데이터 그리드 뷰 비우기
            dgView_market.Rows.Clear();

            // json 타입 리턴값 String 파싱
            string markets = @U.GetMarkets();
            string tick;
            string stat;
            JArray datas = JArray.Parse(markets);
            JArray ticks;
            JArray stats;

            int market_count = 0; // 로딩바를 위한 종목 개수 카운트

            foreach (JObject item in datas)
            {
                Thread.Sleep(50); // For API Call Limit

                string temp = (string)item["market"];

                // BTC, USD 마켓 제외
                if (temp.StartsWith("BTC") || temp.StartsWith("USD"))
                {
                    continue;
                }
                
                // 원화 마켓
                else
                {
                    market_count++;
                    if (market_count < 100)
                    {
                        s.UpdateBar(1);
                    }
                    tick = @U.GetTicks(temp);
                    stat = @U.GetTicker(temp);
                    ticks = JArray.Parse(tick);
                    stats = JArray.Parse(stat);

                    foreach (JObject tick_ in ticks)
                    {
                        foreach (JObject stat_ in stats)
                        {
                            // 데이터 String 가공
                            string change_rate = String.Format("{0:N2}", ((double)stat_["trade_price"] - (double)stat_["opening_price"]) * 100 / (double)stat_["opening_price"]);
                            string change_price;
                            string volume_ = String.Format("{0:N0}", stat_["acc_trade_price_24h"]);
                            string price_;

                            if ((double)tick_["trade_price"] < 10)
                            {
                                price_ = String.Format("{0:N4}", tick_["trade_price"]);
                                change_price = String.Format("{0:N4}", stat_["signed_change_price"]);
                            }
                            else if ((double)tick_["trade_price"] >= 10 && ((double)tick_["trade_price"] < 100))
                            {
                                price_ = String.Format("{0:N1}", tick_["trade_price"]);
                                change_price = String.Format("{0:N1}", stat_["signed_change_price"]);
                            }
                            else
                            {
                                price_ = String.Format("{0:N0}", tick_["trade_price"]);
                                change_price = String.Format("{0:N0}", stat_["signed_change_price"]);
                            }

                            if ((string)stat_["change"] == "RISE")
                            {
                                change_rate = '+' + change_rate;
                                change_price = '+' + change_price;
                            }
                            dgView_market.Rows.Add(item["korean_name"], price_, change_price, change_rate + '%', tick_["market"], volume_ + " 원");     // 열 추가
                        }
                    }
                    // 시세에 따른 글자색 설정
                    foreach (DataGridViewRow dgvRow in dgView_market.Rows)
                    {
                        if (dgvRow.Cells[2].Value.ToString().StartsWith("-"))           // 하락 - Blue
                        {
                            dgvRow.Cells[1].Style.ForeColor = Color.Blue;
                            dgvRow.Cells[2].Style.ForeColor = Color.Blue;
                            dgvRow.Cells[3].Style.ForeColor = Color.Blue;
                        }
                        else if (dgvRow.Cells[2].Value.ToString().StartsWith("+"))  // 상승 - Red
                        {
                            dgvRow.Cells[1].Style.ForeColor = Color.Red;
                            dgvRow.Cells[2].Style.ForeColor = Color.Red;
                            dgvRow.Cells[3].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            s.UpdateBar(0);
            EventPrinter.Items.Add("(" + DateTime.Now.ToLongTimeString().Replace("오전 ", "").Replace("오후 ", "") + ")  " + "종목 현황 업데이트");
            IsMarketLoaded = 1;
            UpdateTime.Text = DateTime.Now.ToLongTimeString();
            #endregion 
        }

        public void Load_InvestTab()
        {
            #region 투자내역 탭 로드
            dgView_my.Rows.Clear();
            string account_data = U.GetAccount();
            JArray values = JArray.Parse(account_data);

            int init_ = 0;
            string buy_price_;
            string now_price = null;
            string change_rate = null;
            string change_krw = null;
            double total_rep = 0;

            foreach (JObject item in values)
            {
                // 첫 번째 데이터는 원화
                if (init_ == 0)
                {
                    buy.Text = String.Format("{0:N0}", Convert.ToDouble(item["balance"])) + " KRW";
                    total_rep += (double)item["balance"];
                    init_++;
                    continue;
                }
                // 거래불가 토큰 예외 처리
                if (Convert.ToDouble(item["avg_buy_price"]) == 0) 
                {
                    continue;
                }

                string ticks = @U.GetTicker("KRW-" + item["currency"]);
                JArray datas = JArray.Parse(ticks);

                foreach (JObject data in datas)
                {
                    now_price = String.Format("{0:N0}", (double)data["trade_price"] * (double)item["balance"]);
                    change_rate = String.Format("{0:N2}", ((double)data["trade_price"] - (double)item["avg_buy_price"]) * 100 / (double)item["avg_buy_price"]) + "%";

                    if ((double)data["trade_price"] < 10)
                    {
                        change_krw = String.Format("{0:N4}", ((double)data["trade_price"] - (double)item["avg_buy_price"]) * (double)item["balance"]);
                    }
                    else if ((double)data["trade_price"] >= 10 && ((double)data["trade_price"] < 100))
                    {
                        change_krw = String.Format("{0:N1}", ((double)data["trade_price"] - (double)item["avg_buy_price"]) * (double)item["balance"]);
                    }
                    else
                    {
                        change_krw = String.Format("{0:N0}", ((double)data["trade_price"] - (double)item["avg_buy_price"]) * (double)item["balance"]);
                    }

                    if (!change_rate.StartsWith("-"))
                    {
                        change_rate = "+" + change_rate;
                        change_krw = "+" + change_krw;
                    }

                    if (now_price != null)
                    {
                        break;
                    }
                }
                buy_price_ = String.Format("{0:N0}", Convert.ToDouble(item["avg_buy_price"]) * Convert.ToDouble(item["balance"]));
                dgView_my.Rows.Add(item["currency"], item["balance"], item["avg_buy_price"], buy_price_, now_price, change_rate, change_krw);
                init_++;
            }

            // 투자 내역이 없는 경우
            if (dgView_my.Rows.Count == 0)
            {
                profit_.Text = "-";
                profit.Text = "-";
                money.Text = "-";
                return;
            }

            double rep_krw = 0;
            double rep_rate = 0;

            foreach (DataGridViewRow dgvRow in dgView_my.Rows)
            {
                if (dgvRow.Cells[5].Value.ToString().StartsWith("-"))           // 하락 - Blue
                {
                    total_rep += Convert.ToDouble(dgvRow.Cells[4].Value);
                    rep_rate += Convert.ToDouble(dgvRow.Cells[5].Value.ToString().Replace("%", ""));
                    rep_krw += Convert.ToDouble(dgvRow.Cells[6].Value);
                    dgvRow.Cells[5].Style.ForeColor = Color.Blue;
                    dgvRow.Cells[6].Style.ForeColor = Color.Blue;
                }
                else if (dgvRow.Cells[5].Value.ToString().StartsWith("+"))   // 상승 - Red
                {
                    total_rep += Convert.ToDouble(dgvRow.Cells[4].Value.ToString().Replace("+", ""));
                    rep_rate += Convert.ToDouble(dgvRow.Cells[5].Value.ToString().Replace("%", "").Replace("+", ""));
                    rep_krw += Convert.ToDouble(dgvRow.Cells[6].Value.ToString().Replace("+", ""));
                    dgvRow.Cells[5].Style.ForeColor = Color.Red;
                    dgvRow.Cells[6].Style.ForeColor = Color.Red;
                }
            }

            total_rep = total_rep / dgView_my.Rows.Count;
            rep_rate = rep_rate / dgView_my.Rows.Count;
            rep_krw = rep_krw / dgView_my.Rows.Count;

            money.Text = String.Format("{0:N0}", total_rep) + " KRW";
            profit_.Text = String.Format("{0:N2}", rep_rate) + " %";
            profit.Text = String.Format("{0:N0}", rep_krw) + " KRW";

            if (profit_.Text.StartsWith("-") && profit.Text.StartsWith("-"))
            {
                profit_.ForeColor = Color.Blue;
                profit.ForeColor = Color.Blue;
            }
            else if (!profit_.Text.StartsWith("-") && !profit.Text.StartsWith("-"))
            {
                profit_.Text = "+" + profit_.Text;
                profit.Text = "+" + profit.Text;
                profit_.ForeColor = Color.Red;
                profit.ForeColor = Color.Red;
            }

            #endregion
        }

        public void Load_FavoriteTab()
        {
            #region 관심 종목 탭 로드
            string Date = DateTime.Now.ToLongDateString().Replace("년 ", "-").Replace("월 ", "-").Replace("일 ", "");
            Monitor_Date.Text = Date.Substring(0, Date.Length - 3);

            Monitor_Time.Text = DateTime.Now.ToLongTimeString();
            #endregion
        }

        public void Load_TradePrice(string name)
        {
            #region 호가창 로드
            Price_ListBox.Items.Clear();
            Amount_ListBox.Items.Clear();

            CoinName.Text = name;
            CoinName.ForeColor = Color.Red;

            string Orders = @U.GetOrderbook(name);
            JArray datas = JArray.Parse(Orders);

            string units  = null;
            foreach(JObject order in datas)
            {
                units = Convert.ToString(order["orderbook_units"]);
                if (units != null)
                    break;
            }

            JArray data = JArray.Parse(units);
            int index = 0;
            foreach(JObject order in data.Reverse())
            {
                if (index <= 7)
                {
                    index++;
                    continue;
                }
                Price_ListBox.Items.Add(Convert.ToString(order["ask_price"]));
                Amount_ListBox.Items.Add(Convert.ToString(order["ask_size"]));
            }

            index = 0;
            foreach(JObject order in data)
            {
                if (index == 8)
                {
                    break;
                }
                Price_ListBox.Items.Add(Convert.ToString(order["bid_price"]));
                Amount_ListBox.Items.Add(Convert.ToString(order["bid_size"]));
                index++;
            }
            #endregion
        }

        private void dgView_market_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            #region 시세 목록 클릭
            // 관심 종목 체크 박스 클릭
            if (e.ColumnIndex == 6)
            {
                // 관심 종목 체크 X이면
                if (!Convert.ToBoolean(dgView_market.Rows[e.RowIndex].Cells[6].Value))
                {
                    dgView_market.Rows[e.RowIndex].Cells[6].Value = true;   // 체크 표시
                    
                    //관심 종목 추가
                    dgView_favorite.Rows.Add(dgView_market.Rows[e.RowIndex].Cells[0].Value, dgView_market.Rows[e.RowIndex].Cells[1].Value, dgView_market.Rows[e.RowIndex].Cells[2].Value, dgView_market.Rows[e.RowIndex].Cells[3].Value, dgView_market.Rows[e.RowIndex].Cells[4].Value, dgView_market.Rows[e.RowIndex].Cells[5].Value);
                    EventPrinter.Items.Add("(" + DateTime.Now.ToLongTimeString().Replace("오전 ", "").Replace("오후 ", "") + ")  " + Convert.ToString(dgView_market.Rows[e.RowIndex].Cells[4].Value) + " 관심 종목 추가");
                }
                // 관심 종목 체크 O이면
                else if (Convert.ToBoolean(dgView_market.Rows[e.RowIndex].Cells[6].Value))
                {
                    dgView_market.Rows[e.RowIndex].Cells[6].Value = false;  // 체크 해제

                    // 관심 종목 삭제
                    foreach (DataGridViewRow dgvRow in dgView_favorite.Rows)
                    {
                        if (dgvRow.Cells[0].Value == dgView_market.Rows[e.RowIndex].Cells[0].Value)
                        {
                            dgView_favorite.Rows.Remove(dgvRow);
                        }
                    }
                    EventPrinter.Items.Add("(" + DateTime.Now.ToLongTimeString().Replace("오전 ", "").Replace("오후 ", "") + ")  " + Convert.ToString(dgView_market.Rows[e.RowIndex].Cells[4].Value) + " 관심 종목 해제");

                }

                foreach (DataGridViewRow dgvRow in dgView_favorite.Rows)
                {
                    if (dgvRow.Cells[2].Value.ToString().StartsWith("-"))           // 하락 - Blue
                    {
                        dgvRow.Cells[1].Style.ForeColor = Color.Blue;
                        dgvRow.Cells[2].Style.ForeColor = Color.Blue;
                        dgvRow.Cells[3].Style.ForeColor = Color.Blue;
                    }
                    else if (dgvRow.Cells[2].Value.ToString().StartsWith("+"))  // 상승 - Red
                    {
                        dgvRow.Cells[1].Style.ForeColor = Color.Red;
                        dgvRow.Cells[2].Style.ForeColor = Color.Red;
                        dgvRow.Cells[3].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        continue;
                    }
                }
                return;
            }

            string name = Convert.ToString(dgView_market.Rows[e.RowIndex].Cells[4].Value);

            Price_ListBox.DrawItem += Price_ListBox_DrawItem;
            Price_ListBox.DrawMode = DrawMode.OwnerDrawVariable;
            Price_ListBox.ItemHeight = Price_ListBox.Height / 15;

            Amount_ListBox.DrawItem += Amount_ListBox_DrawItem;
            Amount_ListBox.DrawMode = DrawMode.OwnerDrawVariable;
            Amount_ListBox.ItemHeight = Amount_ListBox.Height / 15;

            EventPrinter.Items.Add("(" + DateTime.Now.ToLongTimeString().Replace("오전 ", "").Replace("오후 ", "") + ")  " + Convert.ToString(dgView_market.Rows[e.RowIndex].Cells[4].Value) + " 호가 로드");
            Load_TradePrice(name);
            Load_OrderTab(name);
            #endregion 
        }

        private void Price_ListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            #region 호가창 그리기 : 호가
            if (e.Index < 7)
            {
                e.Graphics.FillRectangle(Brushes.DarkSlateBlue, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            }
            else if (e.Index >= 7)
            {
                e.Graphics.FillRectangle(Brushes.Crimson, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            }
            String value = Price_ListBox.Items[e.Index].ToString();
            Brush brush = Brushes.Black;

            SizeF size = e.Graphics.MeasureString(value, e.Font);
            e.Graphics.DrawString(value, e.Font, brush, e.Bounds.Left + (e.Bounds.Width / 2 - size.Width / 2), e.Bounds.Top + (e.Bounds.Height / 2 - size.Height / 2));
            #endregion
        }

        private void Amount_ListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            #region 호가창 그리기 : 주문량
            if (e.Index < 7)
            {
                e.Graphics.FillRectangle(Brushes.DarkSlateBlue, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            }
            else if (e.Index >= 7)
            {
                e.Graphics.FillRectangle(Brushes.Crimson, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            }
            String value = Amount_ListBox.Items[e.Index].ToString();
            Brush brush = Brushes.Black;

            SizeF size = e.Graphics.MeasureString(value, e.Font);
            e.Graphics.DrawString(value, e.Font, brush, e.Bounds.Left + (e.Bounds.Width / 2 - size.Width / 2), e.Bounds.Top + (e.Bounds.Height / 2 - size.Height / 2));
            #endregion
        }

        public void Load_OrderTab(string market)
        {
            #region 매매 주문창 로드
            string orderinfo = @U.GetOrderChance(market);

            // 가공 : 주문 코인 단위
            unit_buy.Text = market.Replace("KRW-", "");
            unit_sell.Text = market.Replace("KRW-", "");

            // 가공 (1) : 주문 가능 금액
            JObject datas = JObject.Parse(orderinfo);
            string bids = Convert.ToString(datas["bid_account"]);
            string asks = Convert.ToString(datas["ask_account"]);

            // 가공 (2) : 주문 가능 금액
            JObject bid = JObject.Parse(bids);
            JObject ask = JObject.Parse(asks);
            canuse_buy.Text = String.Format("{0:N0}", Convert.ToDouble(bid["balance"])) + " KRW";
            canuse_sell.Text = Convert.ToString(ask["balance"]) + " " + unit_sell.Text;

            // 주문창 초기화
            sell_amount.Clear();
            sell_price.Clear();
            buy_amount.Clear();
            buy_price.Clear();
            #endregion
        }

        private void Price_ListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            #region 호가 선택
            string selected_value = Convert.ToString(Price_ListBox.SelectedItem);
            buy_price.Text = selected_value;
            sell_price.Text = selected_value;
            #endregion
        }

        private void buy_clear_Click(object sender, EventArgs e)
        {
            #region 매수 초기화 버튼
            buy_amount.Clear();
            buy_price.Clear();
            buy_total_price.Text = "-";
            #endregion
        }

        private void sell_clear_Click(object sender, EventArgs e)
        {
            #region 매도 초기화 버튼
            sell_amount.Clear();
            sell_price.Clear();
            sell_total_price.Text = "-";
            #endregion
        }

        private void buy_25_Click(object sender, EventArgs e)
        {
            #region 매수 25% 버튼
            string info = @U.GetOrderChance(CoinName.Text);

            // 가공 (1) : 주문 가능 금액
            JObject datas = JObject.Parse(info);
            string bids = Convert.ToString(datas["bid_account"]);

            // 가공 (2) : 주문 가능 금액
            JObject bid = JObject.Parse(bids);
            double per_25 = Convert.ToDouble(bid["balance"]) / 4;
            double now_price = Convert.ToDouble(Price_ListBox.Items[7]);
            double able_amount = per_25 / now_price;

            // 계산 금액 및 수량 출력
            buy_amount.Text = String.Format("{0:N8}", able_amount);
            buy_total_price.Text = String.Format("{0:N0}", per_25) + " KRW";
            #endregion
        }

        private void buy_50_Click(object sender, EventArgs e)
        {
            #region 매수 50% 버튼
            string info = @U.GetOrderChance(CoinName.Text);

            // 가공 (1) : 주문 가능 금액
            JObject datas = JObject.Parse(info);
            string bids = Convert.ToString(datas["bid_account"]);

            // 가공 (2) : 주문 가능 금액
            JObject bid = JObject.Parse(bids);
            double per_50 = Convert.ToDouble(bid["balance"]) / 2;
            double now_price = Convert.ToDouble(Price_ListBox.Items[7]);
            double able_amount = per_50 / now_price;

            // 계산 금액 및 수량 출력
            buy_amount.Text = String.Format("{0:N8}", able_amount);
            buy_total_price.Text = String.Format("{0:N0}", per_50) + " KRW";
            #endregion
        }

        private void buy_100_Click(object sender, EventArgs e)
        {
            #region 매수 100% 버튼
            string info = @U.GetOrderChance(CoinName.Text);

            // 가공 (1) : 주문 가능 금액
            JObject datas = JObject.Parse(info);
            string bids = Convert.ToString(datas["bid_account"]);

            // 가공 (2) : 주문 가능 금액
            JObject bid = JObject.Parse(bids);
            double per_100 = Convert.ToDouble(bid["balance"]);
            double now_price = Convert.ToDouble(Price_ListBox.Items[7]);
            double able_amount = per_100 / now_price;

            // 계산 금액 및 수량 출력
            buy_amount.Text = String.Format("{0:N8}", able_amount);
            buy_total_price.Text = String.Format("{0:N0}", per_100) + " KRW";
            #endregion
        }

        private void sell_25_Click(object sender, EventArgs e)
        {
            #region 매도 25% 버튼
            string info = @U.GetOrderChance(CoinName.Text);
            string name = CoinName.Text.Replace("KRW-", "");

            // 가공 (1) : 주문 가능 금액
            JObject datas = JObject.Parse(info);
            string bids = Convert.ToString(datas["ask_account"]);

            // 가공 (2) : 주문 가능 금액
            JObject bid = JObject.Parse(bids);
            double per_25 = Convert.ToDouble(bid["balance"]) / 4;
            double now_price = Convert.ToDouble(Price_ListBox.Items[6]);
            double able_amount = per_25 / now_price;

            // 계산 금액 및 수량 출력
            sell_amount.Text = String.Format("{0:N8}", per_25);
            sell_total_price.Text = String.Format("{0:N0}", Convert.ToDouble(sell_price.Text) * per_25) + " KRW";
            #endregion
        }

        private void sell_50_Click(object sender, EventArgs e)
        {
            #region 매도 50% 버튼
            string info = @U.GetOrderChance(CoinName.Text);
            string name = CoinName.Text.Replace("KRW-", "");

            // 가공 (1) : 주문 가능 금액
            JObject datas = JObject.Parse(info);
            string bids = Convert.ToString(datas["ask_account"]);

            // 가공 (2) : 주문 가능 금액
            JObject bid = JObject.Parse(bids);
            double per_50 = Convert.ToDouble(bid["balance"]) / 2;
            double now_price = Convert.ToDouble(Price_ListBox.Items[6]);
            double able_amount = per_50 / now_price;

            // 계산 금액 및 수량 출력
            sell_amount.Text = String.Format("{0:N8}", per_50);
            sell_total_price.Text = String.Format("{0:N0}", Convert.ToDouble(sell_price.Text) * per_50) + " KRW";
            #endregion
        }

        private void sell_100_Click(object sender, EventArgs e)
        {
            #region 매도 100% 버튼
            string info = @U.GetOrderChance(CoinName.Text);
            string name = CoinName.Text.Replace("KRW-", "");

            // 가공 (1) : 주문 가능 금액
            JObject datas = JObject.Parse(info);
            string bids = Convert.ToString(datas["ask_account"]);

            // 가공 (2) : 주문 가능 금액
            JObject bid = JObject.Parse(bids);
            double per_100 = Convert.ToDouble(bid["balance"]);
            double now_price = Convert.ToDouble(Price_ListBox.Items[6]);
            double able_amount = per_100 / now_price;

            // 계산 금액 및 수량 출력
            sell_amount.Text = String.Format("{0:N8}", per_100);
            sell_total_price.Text = String.Format("{0:N0}", Convert.ToDouble(sell_price.Text) * per_100) + " KRW";
            #endregion
        }

        private void sell_button_Click(object sender, EventArgs e)
        {
            #region 매도 버튼 클릭
            if (String.IsNullOrWhiteSpace(sell_price.Text))
            {
                MessageBox.Show("주문 금액을 입력하세요.");
                return;
            }
            else if (String.IsNullOrWhiteSpace(sell_amount.Text))
            {
                MessageBox.Show("주문 수량을 입력하세요.");
                return;
            }

            Order order = new Order();
            string stat = "지정가";
            order.init(unit_sell.Text, stat, "매도", sell_amount.Text, sell_price.Text, sell_total_price.Text);
            order.Show();
            #endregion
        }

        private void buy_button_Click(object sender, EventArgs e)
        {
            #region 매수 버튼 클릭
            if (String.IsNullOrWhiteSpace(buy_price.Text))
            {
                MessageBox.Show("주문 금액을 입력하세요.");
                return;
            }
            else if (String.IsNullOrWhiteSpace(buy_amount.Text))
            {
                MessageBox.Show("주문 수량을 입력하세요.");
                return;
            }

            Order order = new Order();
            string stat = "지정가";
            order.init(unit_buy.Text, stat, "매수", buy_amount.Text, buy_price.Text, buy_total_price.Text);
            order.Show();
            #endregion
        }

        private void buy_marketprice_Click(object sender, EventArgs e)
        {
            #region 시장가 매수 버튼 클릭
            if (String.IsNullOrWhiteSpace(buy_amount.Text))
            {
                MessageBox.Show("주문 수량을 입력하세요.");
                return;
            }

            Order order = new Order();
            string stat = "시장가";
            string market_price;
            market_price = String.Format("{0:N0}", Convert.ToDouble(buy_amount.Text) * Convert.ToDouble(Price_ListBox.Items[6].ToString())) + " KRW";
            order.init(unit_buy.Text, stat, "매수", buy_amount.Text, Price_ListBox.Items[6].ToString(), market_price);
            order.Show();
            #endregion
        }

        private void sell_marketprice_Click(object sender, EventArgs e)
        {
            #region 시장가 매도 버튼 클릭
            if (String.IsNullOrWhiteSpace(sell_amount.Text))
            {
                MessageBox.Show("주문 수량을 입력하세요.");
                return;
            }

            Order order = new Order();
            string stat = "시장가";
            string market_price;
            market_price = String.Format("{0:N0}", Convert.ToDouble(sell_amount.Text) * Convert.ToDouble(Price_ListBox.Items[7].ToString())) + " KRW";
            order.init(unit_sell.Text, stat, "매도", sell_amount.Text, Price_ListBox.Items[7].ToString(), market_price);
            order.Show();
            #endregion
        }

        private void buy_amount_TextChanged(object sender, EventArgs e)
        {
            #region 매수 주문창 주문 수량 변경
            if (String.IsNullOrEmpty(buy_price.Text) || String.IsNullOrEmpty(buy_amount.Text))
            {
                buy_total_price.Text = "-";
                return;
            }
            buy_total_price.Text = String.Format("{0:N0}", Convert.ToDouble(Convert.ToDouble(buy_price.Text) * Convert.ToDouble(buy_amount.Text))) + " KRW";
            #endregion
        }

        private void buy_price_TextChanged(object sender, EventArgs e)
        {
            #region 매수 주문창 주문 금액 변경
            if (String.IsNullOrEmpty(buy_price.Text) || String.IsNullOrEmpty(buy_amount.Text))
            {
                buy_total_price.Text = "-";
                return;
            }
            buy_total_price.Text = String.Format("{0:N0}", Convert.ToDouble(Convert.ToDouble(buy_price.Text) * Convert.ToDouble(buy_amount.Text))) + " KRW";
            #endregion
        }

        private void sell_amount_TextChanged(object sender, EventArgs e)
        {
            #region 매도 주문창 주문 수량 변경
            if (String.IsNullOrEmpty(sell_price.Text) || String.IsNullOrEmpty(sell_amount.Text))
            {
                sell_total_price.Text = "-";
                return;
            }
            sell_total_price.Text = String.Format("{0:N0}", Convert.ToDouble(Convert.ToDouble(sell_price.Text) * Convert.ToDouble(sell_amount.Text))) + " KRW";
            #endregion
        }

        private void sell_price_TextChanged(object sender, EventArgs e)
        {
            #region 매도 주문창 주문 금액 변경
            if (String.IsNullOrEmpty(sell_price.Text) || String.IsNullOrEmpty(sell_amount.Text))
            {
                sell_total_price.Text = "-";
                return;
            }
            sell_total_price.Text = String.Format("{0:N0}", Convert.ToDouble(Convert.ToDouble(sell_price.Text) * Convert.ToDouble(sell_amount.Text))) + " KRW";
            #endregion
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region 매수 / 매도 주문 상태 변경시 초기화
            buy_amount.Clear();
            buy_price.Clear();
            buy_total_price.Text = "-";
            sell_amount.Clear();
            sell_price.Clear();
            sell_total_price.Text = "-";
            #endregion
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            #region 새로고침 버튼 클릭
            int index = tabControl1.SelectedIndex;

            if (index == 0) // 시세 조회 및 주문 탭
            {
                IsMarketLoaded = 0;
                Load_TradeTab();
            }
            else if (index == 1)    // 투자 내역 탭
            {
                Load_InvestTab();
            }
            #endregion
        }

        private void dgView_favorite_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            #region 관심 종목 클릭
            var name = dgView_favorite.Rows[e.RowIndex].Cells[4].Value;

            // 시세 주문 탭으로 이동
            tabControl1.SelectedIndex = 0;

            foreach(DataGridViewRow dgvRow in dgView_market.Rows)
            {
                if (dgvRow.Cells[4].Value == name)
                {
                    dgView_market.CurrentCell = dgvRow.Cells[0];

                    Load_TradePrice(Convert.ToString(name));
                    Load_OrderTab(Convert.ToString(name));
                    return;
                }
            }
            #endregion
        }

        private void StartMonitor_Click(object sender, EventArgs e)
        {
            #region 모니터링 시작
            string[] names = new string[dgView_favorite.RowCount];
            double[] init_prices = new double[dgView_favorite.RowCount];
            int[] check_send = new int[dgView_favorite.RowCount];
            
            double opt_rate = 0;

            if (String.IsNullOrEmpty(customVal.Text))
            {
                if (opt_2.Checked)
                {
                    opt_rate = 2.5;
                }
                else if (opt_5.Checked)
                {
                    opt_rate = 5;
                }
                else if (opt_10.Checked)
                {
                    opt_rate = 10;
                }
            }
            else
            {
                opt_rate = Convert.ToDouble(customVal.Text);
            }

            if (opt_rate == 0)
            {
                MessageBox.Show("알림을 받을 변동폭을 지정하세요.");
                return;
            }

            foreach(DataGridViewRow dgvRow in dgView_favorite.Rows)
            {
                names[dgvRow.Index] = Convert.ToString(dgvRow.Cells[4].Value);
                init_prices[dgvRow.Index] = Convert.ToDouble(dgvRow.Cells[1].Value);
            }

            Monitoring s = new Monitoring();
            s.Show();

            int index = 0;

            while (!s.IsDisposed)
            {
                foreach (string name in names)
                {
                    Thread.Sleep(100);
                    string ticks = @U.GetTicker(name);
                    JArray datas = JArray.Parse(ticks);

                    foreach(JObject data in datas)
                    {
                        dataCollect.Items.Add(DateTime.Now.ToShortTimeString() + "  " + name + "  " + String.Format("{0:N2}", ((double)data["trade_price"] - init_prices[index]) * 100 / init_prices[index]) + "%");
                        if (check_send[index] == 0)         // 문자 메시지를 아직 안 보냈다면
                        {
                            double calc = ((double)data["trade_price"] - init_prices[index]) * 100 / init_prices[index];

                            if (calc >= opt_rate)
                            {
                                SMS.SendMessageAsync(PhoneNum.Text, name + " +" + Convert.ToString(opt_rate) + "% 상승 중");
                                check_send[index] = 1;
                                break;
                            }
                            else if (calc <= -opt_rate)
                            {
                                SMS.SendMessageAsync(PhoneNum.Text, name + " " + Convert.ToString(-opt_rate) + "% 하락 중");
                                check_send[index] = 1;
                                break;
                            }
                        }
                        else if (check_send[index] == 1)    // 문자 메시지를 이미 보낸 적이 있다면
                        {
                            break;
                        }
                    }
                    index++;
                }
                index = 0;

                int checkbySum = 0;
                for (int i = 0; i < check_send.Length; i++)
                {
                    checkbySum += check_send[i];
                }

                if (checkbySum == check_send.Length)
                {
                    s.Close();
                    MessageBox.Show("모든 종목에 대한 알림을 전송했습니다.");
                    return;
                }
            }
        }
        #endregion
    }
}

