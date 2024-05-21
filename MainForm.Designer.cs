namespace Task13_ProcessMining
{
    partial class MainForm
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
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            selectFileButton = new Button();
            varFilterLabel = new Label();
            varFilterNUD = new NumericUpDown();
            actFilterNUD = new NumericUpDown();
            fdFilterNUD = new NumericUpDown();
            bindingSource1 = new BindingSource(components);
            openDFGButton = new Button();
            openPetriNetButton = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            varMaxFilterLabel = new Label();
            flowLayoutPanel2 = new FlowLayoutPanel();
            actFilterLabel = new Label();
            actMaxFilterLabel = new Label();
            flowLayoutPanel3 = new FlowLayoutPanel();
            fdFilterLabel = new Label();
            fdMaxFilterLabel = new Label();
            flowLayoutPanel4 = new FlowLayoutPanel();
            comboBox = new ComboBox();
            progressBar = new ProgressBar();
            ((System.ComponentModel.ISupportInitialize)varFilterNUD).BeginInit();
            ((System.ComponentModel.ISupportInitialize)actFilterNUD).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fdFilterNUD).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            flowLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // selectFileButton
            // 
            selectFileButton.Anchor = AnchorStyles.Top;
            selectFileButton.Font = new Font("Arial", 24F, FontStyle.Regular, GraphicsUnit.Point);
            selectFileButton.Location = new Point(48, 8);
            selectFileButton.Name = "selectFileButton";
            selectFileButton.Size = new Size(184, 68);
            selectFileButton.TabIndex = 0;
            selectFileButton.Text = "Select file";
            selectFileButton.UseVisualStyleBackColor = true;
            selectFileButton.Click += selectFileButton_Click;
            // 
            // varFilterLabel
            // 
            varFilterLabel.Anchor = AnchorStyles.Right;
            varFilterLabel.AutoSize = true;
            varFilterLabel.Font = new Font("Arial", 21.75F, FontStyle.Regular, GraphicsUnit.Point);
            varFilterLabel.Location = new Point(3, 4);
            varFilterLabel.Margin = new Padding(3);
            varFilterLabel.Name = "varFilterLabel";
            varFilterLabel.Size = new Size(58, 33);
            varFilterLabel.TabIndex = 1;
            varFilterLabel.Text = "Var";
            // 
            // varFilterNUD
            // 
            varFilterNUD.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            varFilterNUD.Font = new Font("Arial", 18F, FontStyle.Regular, GraphicsUnit.Point);
            varFilterNUD.Location = new Point(67, 3);
            varFilterNUD.Margin = new Padding(3, 3, 3, 38);
            varFilterNUD.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            varFilterNUD.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            varFilterNUD.Name = "varFilterNUD";
            varFilterNUD.Size = new Size(114, 35);
            varFilterNUD.TabIndex = 2;
            varFilterNUD.Value = new decimal(new int[] { 1, 0, 0, 0 });
            varFilterNUD.ValueChanged += filterNUD_ValueChanged;
            // 
            // actFilterNUD
            // 
            actFilterNUD.Font = new Font("Arial", 18F, FontStyle.Regular, GraphicsUnit.Point);
            actFilterNUD.Location = new Point(66, 3);
            actFilterNUD.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            actFilterNUD.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            actFilterNUD.Name = "actFilterNUD";
            actFilterNUD.Size = new Size(115, 35);
            actFilterNUD.TabIndex = 3;
            actFilterNUD.Value = new decimal(new int[] { 1, 0, 0, 0 });
            actFilterNUD.ValueChanged += filterNUD_ValueChanged;
            // 
            // fdFilterNUD
            // 
            fdFilterNUD.Font = new Font("Arial", 18F, FontStyle.Regular, GraphicsUnit.Point);
            fdFilterNUD.Location = new Point(58, 3);
            fdFilterNUD.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            fdFilterNUD.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            fdFilterNUD.Name = "fdFilterNUD";
            fdFilterNUD.Size = new Size(123, 35);
            fdFilterNUD.TabIndex = 4;
            fdFilterNUD.Value = new decimal(new int[] { 1, 0, 0, 0 });
            fdFilterNUD.ValueChanged += filterNUD_ValueChanged;
            // 
            // openDFGButton
            // 
            openDFGButton.Anchor = AnchorStyles.Top;
            openDFGButton.Font = new Font("Arial", 18F, FontStyle.Regular, GraphicsUnit.Point);
            openDFGButton.Location = new Point(48, 269);
            openDFGButton.Name = "openDFGButton";
            openDFGButton.Size = new Size(184, 64);
            openDFGButton.TabIndex = 5;
            openDFGButton.Text = "Open DFG";
            openDFGButton.UseVisualStyleBackColor = true;
            openDFGButton.Click += openDFGButton_Click;
            // 
            // openPetriNetButton
            // 
            openPetriNetButton.Anchor = AnchorStyles.Top;
            openPetriNetButton.Font = new Font("Arial", 18F, FontStyle.Regular, GraphicsUnit.Point);
            openPetriNetButton.Location = new Point(48, 339);
            openPetriNetButton.Name = "openPetriNetButton";
            openPetriNetButton.Size = new Size(184, 66);
            openPetriNetButton.TabIndex = 6;
            openPetriNetButton.Text = "Open PetriNet";
            openPetriNetButton.UseVisualStyleBackColor = true;
            openPetriNetButton.Click += openPetriNetButton_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Controls.Add(varFilterLabel);
            flowLayoutPanel1.Controls.Add(varFilterNUD);
            flowLayoutPanel1.Controls.Add(varMaxFilterLabel);
            flowLayoutPanel1.Location = new Point(3, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(229, 41);
            flowLayoutPanel1.TabIndex = 7;
            // 
            // varMaxFilterLabel
            // 
            varMaxFilterLabel.Anchor = AnchorStyles.Right;
            varMaxFilterLabel.AutoSize = true;
            varMaxFilterLabel.Font = new Font("Arial", 21.75F, FontStyle.Regular, GraphicsUnit.Point);
            varMaxFilterLabel.Location = new Point(187, 4);
            varMaxFilterLabel.Margin = new Padding(3);
            varMaxFilterLabel.Name = "varMaxFilterLabel";
            varMaxFilterLabel.Size = new Size(39, 33);
            varMaxFilterLabel.TabIndex = 12;
            varMaxFilterLabel.Text = "/1";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSize = true;
            flowLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel2.Controls.Add(actFilterLabel);
            flowLayoutPanel2.Controls.Add(actFilterNUD);
            flowLayoutPanel2.Controls.Add(actMaxFilterLabel);
            flowLayoutPanel2.Location = new Point(3, 50);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(229, 41);
            flowLayoutPanel2.TabIndex = 8;
            // 
            // actFilterLabel
            // 
            actFilterLabel.Anchor = AnchorStyles.Right;
            actFilterLabel.AutoSize = true;
            actFilterLabel.Font = new Font("Arial", 21.75F, FontStyle.Regular, GraphicsUnit.Point);
            actFilterLabel.Location = new Point(3, 4);
            actFilterLabel.Margin = new Padding(3);
            actFilterLabel.Name = "actFilterLabel";
            actFilterLabel.Size = new Size(57, 33);
            actFilterLabel.TabIndex = 1;
            actFilterLabel.Text = "Act";
            // 
            // actMaxFilterLabel
            // 
            actMaxFilterLabel.Anchor = AnchorStyles.Right;
            actMaxFilterLabel.AutoSize = true;
            actMaxFilterLabel.Font = new Font("Arial", 21.75F, FontStyle.Regular, GraphicsUnit.Point);
            actMaxFilterLabel.Location = new Point(187, 4);
            actMaxFilterLabel.Margin = new Padding(3);
            actMaxFilterLabel.Name = "actMaxFilterLabel";
            actMaxFilterLabel.Size = new Size(39, 33);
            actMaxFilterLabel.TabIndex = 11;
            actMaxFilterLabel.Text = "/1";
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.AutoSize = true;
            flowLayoutPanel3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel3.Controls.Add(fdFilterLabel);
            flowLayoutPanel3.Controls.Add(fdFilterNUD);
            flowLayoutPanel3.Controls.Add(fdMaxFilterLabel);
            flowLayoutPanel3.Location = new Point(3, 97);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(229, 41);
            flowLayoutPanel3.TabIndex = 9;
            // 
            // fdFilterLabel
            // 
            fdFilterLabel.Anchor = AnchorStyles.Right;
            fdFilterLabel.AutoSize = true;
            fdFilterLabel.Font = new Font("Arial", 21.75F, FontStyle.Regular, GraphicsUnit.Point);
            fdFilterLabel.Location = new Point(3, 4);
            fdFilterLabel.Margin = new Padding(3);
            fdFilterLabel.Name = "fdFilterLabel";
            fdFilterLabel.Size = new Size(49, 33);
            fdFilterLabel.TabIndex = 1;
            fdFilterLabel.Text = "Fd";
            // 
            // fdMaxFilterLabel
            // 
            fdMaxFilterLabel.Anchor = AnchorStyles.Right;
            fdMaxFilterLabel.AutoSize = true;
            fdMaxFilterLabel.Font = new Font("Arial", 21.75F, FontStyle.Regular, GraphicsUnit.Point);
            fdMaxFilterLabel.Location = new Point(187, 4);
            fdMaxFilterLabel.Margin = new Padding(3);
            fdMaxFilterLabel.Name = "fdMaxFilterLabel";
            fdMaxFilterLabel.Size = new Size(39, 33);
            fdMaxFilterLabel.TabIndex = 12;
            fdMaxFilterLabel.Text = "/1";
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.Anchor = AnchorStyles.Top;
            flowLayoutPanel4.AutoSize = true;
            flowLayoutPanel4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel4.Controls.Add(flowLayoutPanel1);
            flowLayoutPanel4.Controls.Add(flowLayoutPanel2);
            flowLayoutPanel4.Controls.Add(flowLayoutPanel3);
            flowLayoutPanel4.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel4.Location = new Point(27, 122);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(235, 141);
            flowLayoutPanel4.TabIndex = 10;
            // 
            // comboBox
            // 
            comboBox.Anchor = AnchorStyles.Top;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            comboBox.FormattingEnabled = true;
            comboBox.Location = new Point(48, 82);
            comboBox.Name = "comboBox";
            comboBox.Size = new Size(184, 26);
            comboBox.TabIndex = 11;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(12, 411);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(260, 23);
            progressBar.TabIndex = 12;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 441);
            Controls.Add(progressBar);
            Controls.Add(comboBox);
            Controls.Add(flowLayoutPanel4);
            Controls.Add(openPetriNetButton);
            Controls.Add(openDFGButton);
            Controls.Add(selectFileButton);
            Name = "MainForm";
            Text = "Process analysis";
            ((System.ComponentModel.ISupportInitialize)varFilterNUD).EndInit();
            ((System.ComponentModel.ISupportInitialize)actFilterNUD).EndInit();
            ((System.ComponentModel.ISupportInitialize)fdFilterNUD).EndInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel3.PerformLayout();
            flowLayoutPanel4.ResumeLayout(false);
            flowLayoutPanel4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button selectFileButton;
        private Label varFilterLabel;
        private NumericUpDown varFilterNUD;
        private NumericUpDown actFilterNUD;
        private NumericUpDown fdFilterNUD;
        private BindingSource bindingSource1;
        private Button openDFGButton;
        private Button openPetriNetButton;
        private FlowLayoutPanel flowLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label actFilterLabel;
        private FlowLayoutPanel flowLayoutPanel3;
        private Label fdFilterLabel;
        private Label varMaxFilterLabel;
        private Label actMaxFilterLabel;
        private Label fdMaxFilterLabel;
        private FlowLayoutPanel flowLayoutPanel4;
        private ComboBox comboBox;
        private ProgressBar progressBar;
    }
}
