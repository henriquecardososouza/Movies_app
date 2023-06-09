namespace Movies_app.Forms
{
    partial class FrmCadastrar
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
            lblTitle = new Label();
            tbSenha = new TextBox();
            tbUsuario = new TextBox();
            btnCadastrar = new Button();
            pnlUserLine = new Panel();
            pnlSenhaLine = new Panel();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Source Serif Pro", 15.7499981F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = SystemColors.ControlLightLight;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(234, 70);
            lblTitle.TabIndex = 10;
            lblTitle.Text = "Cadastrar";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tbSenha
            // 
            tbSenha.BackColor = SystemColors.InfoText;
            tbSenha.BorderStyle = BorderStyle.None;
            tbSenha.ForeColor = SystemColors.Info;
            tbSenha.Location = new Point(13, 139);
            tbSenha.Name = "tbSenha";
            tbSenha.PasswordChar = '*';
            tbSenha.PlaceholderText = "Senha...";
            tbSenha.Size = new Size(209, 16);
            tbSenha.TabIndex = 8;
            tbSenha.Enter += tbSenha_Enter;
            tbSenha.Leave += tbSenha_Leave;
            // 
            // tbUsuario
            // 
            tbUsuario.BackColor = SystemColors.MenuText;
            tbUsuario.BorderStyle = BorderStyle.None;
            tbUsuario.ForeColor = SystemColors.Window;
            tbUsuario.Location = new Point(13, 79);
            tbUsuario.Name = "tbUsuario";
            tbUsuario.PlaceholderText = "Usuário...";
            tbUsuario.Size = new Size(209, 16);
            tbUsuario.TabIndex = 7;
            tbUsuario.Enter += tbUsuario_Enter;
            tbUsuario.Leave += tbUsuario_Leave;
            // 
            // btnCadastrar
            // 
            btnCadastrar.FlatAppearance.BorderColor = Color.FromArgb(40, 40, 40);
            btnCadastrar.FlatAppearance.BorderSize = 0;
            btnCadastrar.FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 20, 20);
            btnCadastrar.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 40);
            btnCadastrar.FlatStyle = FlatStyle.Flat;
            btnCadastrar.ForeColor = SystemColors.ControlLightLight;
            btnCadastrar.Location = new Point(147, 192);
            btnCadastrar.Name = "btnCadastrar";
            btnCadastrar.Size = new Size(75, 23);
            btnCadastrar.TabIndex = 6;
            btnCadastrar.Text = "Cadastrar";
            btnCadastrar.UseVisualStyleBackColor = true;
            btnCadastrar.MouseClick += btnCadastrar_MouseClick;
            // 
            // pnlUserLine
            // 
            pnlUserLine.BackColor = Color.FromArgb(50, 50, 50);
            pnlUserLine.Location = new Point(13, 101);
            pnlUserLine.Name = "pnlUserLine";
            pnlUserLine.Size = new Size(209, 1);
            pnlUserLine.TabIndex = 11;
            // 
            // pnlSenhaLine
            // 
            pnlSenhaLine.BackColor = Color.FromArgb(50, 50, 50);
            pnlSenhaLine.Location = new Point(13, 161);
            pnlSenhaLine.Name = "pnlSenhaLine";
            pnlSenhaLine.Size = new Size(209, 1);
            pnlSenhaLine.TabIndex = 12;
            // 
            // FrmCadastrar
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(234, 228);
            Controls.Add(pnlSenhaLine);
            Controls.Add(pnlUserLine);
            Controls.Add(lblTitle);
            Controls.Add(tbSenha);
            Controls.Add(tbUsuario);
            Controls.Add(btnCadastrar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmCadastrar";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cadastrar";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private TextBox tbSenha;
        private TextBox tbUsuario;
        private Button btnCadastrar;
        private Panel pnlUserLine;
        private Panel pnlSenhaLine;
    }
}