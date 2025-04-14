namespace Task2_1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        [Obsolete]
        private void InitializeComponent()
        {
            glControl1 = new OpenTK.GLControl.GLControl();
            mainMenu = new GroupBox();
            level5Btn = new Button();
            level4Btn = new Button();
            level3Btn = new Button();
            level2Btn = new Button();
            level1Btn = new Button();
            title = new Label();
            mainMenu.SuspendLayout();
            SuspendLayout();
            // 
            // glControl1
            // 
            glControl1.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            glControl1.APIVersion = new Version(3, 3, 0, 0);
            glControl1.Dock = DockStyle.Fill;
            glControl1.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            glControl1.IsEventDriven = true;
            glControl1.Location = new Point(0, 0);
            glControl1.Name = "glControl1";
            glControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            glControl1.SharedContext = null;
            glControl1.Size = new Size(882, 853);
            glControl1.TabIndex = 0;
            glControl1.Load += GLControlLoad;
            glControl1.Paint += GLControlPaint;
            glControl1.MouseDown += GLControlMouseDown;
            glControl1.Resize += GLControlResize;
            // 
            // mainMenu
            // 
            mainMenu.Controls.Add(level5Btn);
            mainMenu.Controls.Add(level4Btn);
            mainMenu.Controls.Add(level3Btn);
            mainMenu.Controls.Add(level2Btn);
            mainMenu.Controls.Add(level1Btn);
            mainMenu.Controls.Add(title);
            mainMenu.Dock = DockStyle.Top;
            mainMenu.Location = new Point(0, 0);
            mainMenu.Name = "mainMenu";
            mainMenu.Size = new Size(882, 335);
            mainMenu.TabIndex = 1;
            mainMenu.TabStop = false;
            mainMenu.Text = "groupBox1";
            // 
            // level5Btn
            // 
            level5Btn.Anchor = AnchorStyles.Top;
            level5Btn.Location = new Point(720, 150);
            level5Btn.Name = "level5Btn";
            level5Btn.Size = new Size(94, 29);
            level5Btn.TabIndex = 5;
            level5Btn.Text = "5";
            level5Btn.UseVisualStyleBackColor = true;
            level5Btn.Click += OnDifficultBtnDown;
            // 
            // level4Btn
            // 
            level4Btn.Anchor = AnchorStyles.Top;
            level4Btn.Location = new Point(551, 150);
            level4Btn.Name = "level4Btn";
            level4Btn.Size = new Size(94, 29);
            level4Btn.TabIndex = 4;
            level4Btn.Text = "4";
            level4Btn.UseVisualStyleBackColor = true;
            level4Btn.Click += OnDifficultBtnDown;
            // 
            // level3Btn
            // 
            level3Btn.Anchor = AnchorStyles.Top;
            level3Btn.Location = new Point(376, 150);
            level3Btn.Name = "level3Btn";
            level3Btn.Size = new Size(94, 29);
            level3Btn.TabIndex = 3;
            level3Btn.Text = "3";
            level3Btn.UseVisualStyleBackColor = true;
            level3Btn.Click += OnDifficultBtnDown;
            // 
            // level2Btn
            // 
            level2Btn.Anchor = AnchorStyles.Top;
            level2Btn.Location = new Point(219, 150);
            level2Btn.Name = "level2Btn";
            level2Btn.Size = new Size(94, 29);
            level2Btn.TabIndex = 2;
            level2Btn.Text = "2";
            level2Btn.UseVisualStyleBackColor = true;
            level2Btn.Click += OnDifficultBtnDown;
            // 
            // level1Btn
            // 
            level1Btn.Anchor = AnchorStyles.Top;
            level1Btn.Location = new Point(67, 150);
            level1Btn.Name = "level1Btn";
            level1Btn.Size = new Size(94, 29);
            level1Btn.TabIndex = 1;
            level1Btn.Text = "1";
            level1Btn.UseVisualStyleBackColor = true;
            level1Btn.Click += OnDifficultBtnDown;
            // 
            // title
            // 
            title.Anchor = AnchorStyles.Top;
            title.AutoSize = true;
            title.Location = new Point(346, 23);
            title.Name = "title";
            title.Size = new Size(156, 20);
            title.TabIndex = 0;
            title.Text = "Выберите сложность";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(882, 853);
            Controls.Add(mainMenu);
            Controls.Add(glControl1);
            Name = "Form1";
            Text = "Form1";
            mainMenu.ResumeLayout(false);
            mainMenu.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private OpenTK.GLControl.GLControl glControl1;
        private GroupBox mainMenu;
        private Button level5Btn;
        private Button level4Btn;
        private Button level3Btn;
        private Button level2Btn;
        private Button level1Btn;
        private Label title;
    }
}
