namespace Movies_app.Forms
{
    partial class FrmLogin
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
            btnEntrar = new Button();
            tbUsuario = new TextBox();
            tbSenha = new TextBox();
            lblNaoPossuiConta = new Label();
            lblTitle = new Label();
            lblCadastrar = new Label();
            pnlUserLine = new Panel();
            pnlSenhaLine = new Panel();
            SuspendLayout();
            // 
            // btnEntrar
            // 
            btnEntrar.FlatAppearance.BorderColor = Color.FromArgb(40, 40, 40);
            btnEntrar.FlatAppearance.BorderSize = 0;
            btnEntrar.FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 20, 20);
            btnEntrar.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 40);
            btnEntrar.FlatStyle = FlatStyle.Flat;
            btnEntrar.ForeColor = SystemColors.ControlLightLight;
            btnEntrar.Location = new Point(147, 210);
            btnEntrar.Name = "btnEntrar";
            btnEntrar.Size = new Size(75, 23);
            btnEntrar.TabIndex = 0;
            btnEntrar.Text = "Entrar";
            btnEntrar.UseVisualStyleBackColor = true;
            btnEntrar.MouseClick += btnEntrar_MouseClick;
            // 
            // tbUsuario
            // 
            tbUsuario.BackColor = SystemColors.MenuText;
            tbUsuario.BorderStyle = BorderStyle.None;
            tbUsuario.ForeColor = SystemColors.Window;
            tbUsuario.Location = new Point(13, 73);
            tbUsuario.Name = "tbUsuario";
            tbUsuario.PlaceholderText = "Usuário...";
            tbUsuario.Size = new Size(209, 16);
            tbUsuario.TabIndex = 1;
            tbUsuario.Enter += tbUsuario_Enter;
            tbUsuario.Leave += tbUsuario_Leave;
            // 
            // tbSenha
            // 
            tbSenha.BackColor = SystemColors.InfoText;
            tbSenha.BorderStyle = BorderStyle.None;
            tbSenha.ForeColor = SystemColors.Info;
            tbSenha.Location = new Point(13, 133);
            tbSenha.Name = "tbSenha";
            tbSenha.PasswordChar = '*';
            tbSenha.PlaceholderText = "Senha...";
            tbSenha.Size = new Size(209, 16);
            tbSenha.TabIndex = 2;
            tbSenha.Enter += tbSenha_Enter;
            tbSenha.Leave += tbSenha_Leave;
            // 
            // lblNaoPossuiConta
            // 
            lblNaoPossuiConta.AutoSize = true;
            lblNaoPossuiConta.ForeColor = SystemColors.ControlLightLight;
            lblNaoPossuiConta.Location = new Point(12, 159);
            lblNaoPossuiConta.Name = "lblNaoPossuiConta";
            lblNaoPossuiConta.Size = new Size(104, 15);
            lblNaoPossuiConta.TabIndex = 3;
            lblNaoPossuiConta.Text = "Não possui conta?";
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Source Serif Pro", 15.7499981F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = SystemColors.ControlLightLight;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(234, 70);
            lblTitle.TabIndex = 4;
            lblTitle.Text = "Entrar";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCadastrar
            // 
            lblCadastrar.AutoSize = true;
            lblCadastrar.Cursor = Cursors.Hand;
            lblCadastrar.ForeColor = SystemColors.ControlLightLight;
            lblCadastrar.Location = new Point(149, 159);
            lblCadastrar.Name = "lblCadastrar";
            lblCadastrar.Size = new Size(73, 15);
            lblCadastrar.TabIndex = 5;
            lblCadastrar.Text = "Cadastrar-se";
            lblCadastrar.MouseClick += lblCadastrar_MouseClick;
            lblCadastrar.MouseEnter += lblCadastrar_MouseEnter;
            lblCadastrar.MouseLeave += lblCadastrar_MouseLeave;
            // 
            // pnlUserLine
            // 
            pnlUserLine.BackColor = Color.FromArgb(50, 50, 50);
            pnlUserLine.Location = new Point(13, 95);
            pnlUserLine.Name = "pnlUserLine";
            pnlUserLine.Size = new Size(209, 1);
            pnlUserLine.TabIndex = 12;
            // 
            // pnlSenhaLine
            // 
            pnlSenhaLine.BackColor = Color.FromArgb(50, 50, 50);
            pnlSenhaLine.Location = new Point(13, 155);
            pnlSenhaLine.Name = "pnlSenhaLine";
            pnlSenhaLine.Size = new Size(209, 1);
            pnlSenhaLine.TabIndex = 13;
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(234, 245);
            Controls.Add(pnlSenhaLine);
            Controls.Add(pnlUserLine);
            Controls.Add(lblCadastrar);
            Controls.Add(lblTitle);
            Controls.Add(lblNaoPossuiConta);
            Controls.Add(tbSenha);
            Controls.Add(tbUsuario);
            Controls.Add(btnEntrar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmLogin";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Entrar";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnEntrar;
        private TextBox tbUsuario;
        private TextBox tbSenha;
        private Label lblNaoPossuiConta;
        private Label lblTitle;
        private Label lblCadastrar;
        private Panel pnlUserLine;
        private Panel pnlSenhaLine;
    }
}