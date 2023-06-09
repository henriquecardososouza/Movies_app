namespace Movies_app.Forms
{
    partial class FrmEnterKey
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
            tbChave = new TextBox();
            btnVerificar = new Button();
            lblChaveApi = new Label();
            lblLinkMessage = new Label();
            lblLink = new Label();
            pnlLine = new Panel();
            SuspendLayout();
            // 
            // tbChave
            // 
            tbChave.BackColor = Color.Black;
            tbChave.BorderStyle = BorderStyle.None;
            tbChave.ForeColor = Color.White;
            tbChave.Location = new Point(10, 42);
            tbChave.Name = "tbChave";
            tbChave.PlaceholderText = "Chave...";
            tbChave.Size = new Size(221, 16);
            tbChave.TabIndex = 0;
            // 
            // btnVerificar
            // 
            btnVerificar.FlatAppearance.BorderSize = 0;
            btnVerificar.FlatAppearance.MouseDownBackColor = Color.FromArgb(10, 10, 10);
            btnVerificar.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
            btnVerificar.FlatStyle = FlatStyle.Flat;
            btnVerificar.ForeColor = Color.White;
            btnVerificar.Location = new Point(156, 106);
            btnVerificar.Name = "btnVerificar";
            btnVerificar.Size = new Size(75, 23);
            btnVerificar.TabIndex = 1;
            btnVerificar.Text = "Verificar";
            btnVerificar.UseVisualStyleBackColor = true;
            btnVerificar.MouseClick += btnVerificar_MouseClick;
            // 
            // lblChaveApi
            // 
            lblChaveApi.AutoSize = true;
            lblChaveApi.ForeColor = Color.White;
            lblChaveApi.Location = new Point(10, 20);
            lblChaveApi.Name = "lblChaveApi";
            lblChaveApi.Size = new Size(150, 15);
            lblChaveApi.TabIndex = 3;
            lblChaveApi.Text = "Informe uma chave da API:";
            // 
            // lblLinkMessage
            // 
            lblLinkMessage.AutoSize = true;
            lblLinkMessage.ForeColor = Color.White;
            lblLinkMessage.Location = new Point(45, 68);
            lblLinkMessage.Name = "lblLinkMessage";
            lblLinkMessage.Size = new Size(132, 15);
            lblLinkMessage.TabIndex = 4;
            lblLinkMessage.Text = "Não possui uma chave?";
            // 
            // lblLink
            // 
            lblLink.AutoSize = true;
            lblLink.Cursor = Cursors.Hand;
            lblLink.ForeColor = Color.White;
            lblLink.Location = new Point(174, 68);
            lblLink.Name = "lblLink";
            lblLink.Size = new Size(57, 15);
            lblLink.TabIndex = 5;
            lblLink.Text = "Cadastrar";
            lblLink.MouseClick += lblLink_MouseClick;
            lblLink.MouseEnter += lblLink_MouseEnter;
            lblLink.MouseLeave += lblLink_MouseLeave;
            // 
            // pnlLine
            // 
            pnlLine.BackColor = Color.White;
            pnlLine.Location = new Point(12, 63);
            pnlLine.Name = "pnlLine";
            pnlLine.Size = new Size(219, 1);
            pnlLine.TabIndex = 6;
            // 
            // FrmEnterKey
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(243, 141);
            Controls.Add(pnlLine);
            Controls.Add(lblLink);
            Controls.Add(lblLinkMessage);
            Controls.Add(lblChaveApi);
            Controls.Add(btnVerificar);
            Controls.Add(tbChave);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmEnterKey";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Chave da API";
            FormClosing += FrmEnterKey_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbChave;
        private Button btnVerificar;
        private Label lblChaveApi;
        private Label lblLinkMessage;
        private Label lblLink;
        private Panel pnlLine;
    }
}