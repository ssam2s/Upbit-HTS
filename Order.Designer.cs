namespace GUI_FINAL_PROJECT
{
    partial class Order
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.check_title = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.check_name = new System.Windows.Forms.Label();
            this.check_buy_sell = new System.Windows.Forms.Label();
            this.check_amount = new System.Windows.Forms.Label();
            this.check_price = new System.Windows.Forms.Label();
            this.check_price_total = new System.Windows.Forms.Label();
            this.check_order_stat = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // check_title
            // 
            this.check_title.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.check_title.AutoSize = true;
            this.check_title.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.check_title.Location = new System.Drawing.Point(74, 25);
            this.check_title.Name = "check_title";
            this.check_title.Size = new System.Drawing.Size(200, 27);
            this.check_title.TabIndex = 0;
            this.check_title.Text = "매수 주문 확인";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 14F);
            this.label2.Location = new System.Drawing.Point(12, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "주문코인 :";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 14F);
            this.label3.Location = new System.Drawing.Point(12, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "주문구분 :";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 14F);
            this.label4.Location = new System.Drawing.Point(12, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 19);
            this.label4.TabIndex = 3;
            this.label4.Text = "주문수량 :";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 14F);
            this.label5.Location = new System.Drawing.Point(12, 216);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 19);
            this.label5.TabIndex = 4;
            this.label5.Text = "주문가격 :";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 14F);
            this.label6.Location = new System.Drawing.Point(12, 258);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 19);
            this.label6.TabIndex = 5;
            this.label6.Text = "주문총액 :";
            // 
            // check_name
            // 
            this.check_name.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.check_name.Font = new System.Drawing.Font("굴림", 14F);
            this.check_name.Location = new System.Drawing.Point(115, 85);
            this.check_name.Name = "check_name";
            this.check_name.Size = new System.Drawing.Size(217, 19);
            this.check_name.TabIndex = 6;
            this.check_name.Text = "-";
            this.check_name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // check_buy_sell
            // 
            this.check_buy_sell.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.check_buy_sell.Font = new System.Drawing.Font("굴림", 14F);
            this.check_buy_sell.Location = new System.Drawing.Point(213, 129);
            this.check_buy_sell.Name = "check_buy_sell";
            this.check_buy_sell.Size = new System.Drawing.Size(119, 19);
            this.check_buy_sell.TabIndex = 7;
            this.check_buy_sell.Text = "-";
            this.check_buy_sell.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // check_amount
            // 
            this.check_amount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.check_amount.Font = new System.Drawing.Font("굴림", 14F);
            this.check_amount.Location = new System.Drawing.Point(115, 172);
            this.check_amount.Name = "check_amount";
            this.check_amount.Size = new System.Drawing.Size(217, 19);
            this.check_amount.TabIndex = 8;
            this.check_amount.Text = "-";
            this.check_amount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // check_price
            // 
            this.check_price.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.check_price.Font = new System.Drawing.Font("굴림", 14F);
            this.check_price.Location = new System.Drawing.Point(115, 216);
            this.check_price.Name = "check_price";
            this.check_price.Size = new System.Drawing.Size(217, 19);
            this.check_price.TabIndex = 9;
            this.check_price.Text = "-";
            this.check_price.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // check_price_total
            // 
            this.check_price_total.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.check_price_total.Font = new System.Drawing.Font("굴림", 14F);
            this.check_price_total.Location = new System.Drawing.Point(115, 258);
            this.check_price_total.Name = "check_price_total";
            this.check_price_total.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.check_price_total.Size = new System.Drawing.Size(217, 19);
            this.check_price_total.TabIndex = 10;
            this.check_price_total.Text = "-";
            this.check_price_total.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // check_order_stat
            // 
            this.check_order_stat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.check_order_stat.Font = new System.Drawing.Font("굴림", 14F);
            this.check_order_stat.Location = new System.Drawing.Point(135, 129);
            this.check_order_stat.Name = "check_order_stat";
            this.check_order_stat.Size = new System.Drawing.Size(139, 19);
            this.check_order_stat.TabIndex = 11;
            this.check_order_stat.Text = "-";
            this.check_order_stat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(16, 301);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(114, 48);
            this.cancel.TabIndex = 12;
            this.cancel.Text = "취소";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(218, 301);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(114, 48);
            this.button2.TabIndex = 13;
            this.button2.Text = "확인";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Order
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 361);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.check_order_stat);
            this.Controls.Add(this.check_price_total);
            this.Controls.Add(this.check_price);
            this.Controls.Add(this.check_amount);
            this.Controls.Add(this.check_buy_sell);
            this.Controls.Add(this.check_name);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.check_title);
            this.Name = "Order";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "주문 확인";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label check_title;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label check_name;
        private System.Windows.Forms.Label check_buy_sell;
        private System.Windows.Forms.Label check_amount;
        private System.Windows.Forms.Label check_price;
        private System.Windows.Forms.Label check_price_total;
        private System.Windows.Forms.Label check_order_stat;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button button2;
    }
}