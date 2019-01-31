using System;
using System.Drawing;
using System.Windows.Forms;

using Liven.Data;


namespace Liven.Forms {
  public partial class ResGenForm : Form {
    private MenuStrip mMain;
    private ToolStripMenuItem mFile;
    private ToolStripMenuItem mfNew, mfOpen, mfClose, mfSave, mfSaveAs, mfExit;

    private ToolStrip mTool;
    private ToolStripDropDownButton mbtnType;

#if ENABLE_AUDIO
    private ToolStripMenuItem mtString, mtIcon, mtImage, mtAudio, mtFile;
    private ListView lvIcon, lvImage, lvAudio, lvFile;
#else
    private ToolStripMenuItem mtString, mtIcon, mtImage, mtFile;
    private ListView lvIcon, lvImage, lvFile;
#endif

    private ToolStripButton mbtnAdd;
    private ToolStripButton mbtnDelete;

    private Panel pnlContent;

    private DataGridView dgvString;


    //
#region private void InitializeComponent()
    private void InitializeComponent() {
      this.mMain = new MenuStrip();
      this.mFile    = new ToolStripMenuItem();
      this.mfNew    = new ToolStripMenuItem();
      this.mfOpen   = new ToolStripMenuItem();
      this.mfClose  = new ToolStripMenuItem();
      this.mfSave   = new ToolStripMenuItem();
      this.mfSaveAs = new ToolStripMenuItem();
      this.mfExit   = new ToolStripMenuItem();

      this.mTool    = new ToolStrip();
      this.mbtnType = new ToolStripDropDownButton();
      this.mtString = new ToolStripMenuItem();
      this.mtIcon   = new ToolStripMenuItem();
      this.mtImage  = new ToolStripMenuItem();
#if ENABLE_AUDIO
      this.mtAudio  = new ToolStripMenuItem();
#endif
      this.mtFile   = new ToolStripMenuItem();
      this.mbtnAdd    = new ToolStripButton();
      this.mbtnDelete = new ToolStripButton();

      this.pnlContent = new Panel();
      this.dgvString = new DataGridView();
      this.lvIcon    = new ListView();
      this.lvImage   = new ListView();
#if ENABLE_AUDIO
      this.lvAudio   = new ListView();
#endif
      this.lvFile    = new ListView();


      this.SuspendLayout();

      this.Controls.Add(this.pnlContent);
      this.Controls.Add(this.mTool);
      this.Controls.Add(this.mMain);

      this.mMain.Items.Add(this.mFile);
      //this.mFile.DropDownItems.Add(this.mfNew);
      this.mFile.DropDownItems.Add(this.mfOpen);
      this.mFile.DropDownItems.Add(this.mfClose);
      this.mFile.DropDownItems.Add(new ToolStripSeparator());
      this.mFile.DropDownItems.Add(this.mfSave);
      this.mFile.DropDownItems.Add(this.mfSaveAs);
      this.mFile.DropDownItems.Add(new ToolStripSeparator());
      this.mFile.DropDownItems.Add(this.mfExit);


      this.mTool.Items.Add(this.mbtnType);
      this.mTool.Items.Add(this.mbtnAdd);
      this.mTool.Items.Add(this.mbtnDelete);

      this.mbtnType.DropDownItems.Add(this.mtString);
      this.mbtnType.DropDownItems.Add(this.mtIcon);
      this.mbtnType.DropDownItems.Add(this.mtImage);
#if ENABLE_AUDIO
      this.mbtnType.DropDownItems.Add(this.mtAudio);
#endif
      this.mbtnType.DropDownItems.Add(this.mtFile);

      this.pnlContent.Controls.Add(this.dgvString);
      this.pnlContent.Controls.Add(this.lvIcon);
      this.pnlContent.Controls.Add(this.lvImage);
#if ENABLE_AUDIO
      this.pnlContent.Controls.Add(this.lvAudio);
#endif
      this.pnlContent.Controls.Add(this.lvFile);

      // Main Menu
      // �t�@�C�����j���[
      this.mFile.Text = "�t�@�C��(&F)";
      //
      this.mfNew.Text                = "���\�[�X�t�@�C�����쐬(&N)";
      this.mfNew.ShortcutKeys        = Keys.Control | Keys.N;
      this.mfNew.ShowShortcutKeys    = true;
      //
      this.mfOpen.Text               = "�J��(&O)";
      this.mfOpen.ShortcutKeys       = Keys.Control | Keys.O;
      this.mfNew.ShowShortcutKeys    = true;
      //
      this.mfClose.Text              = "����(&C)";
      this.mfClose.ShortcutKeys      = Keys.Control | Keys.W;
      this.mfClose.ShowShortcutKeys  = true;
      this.mfClose.Enabled           = false;
      //
      this.mfSave.Text               = "�ۑ�(&S)";
      this.mfSave.ShortcutKeys       = Keys.Control | Keys.S;
      this.mfSave.ShowShortcutKeys   = true;
      //
      this.mfSaveAs.Text             = "���O��t���ĕۑ�";
      this.mfSaveAs.ShortcutKeys     = Keys.Control | Keys.Shift | Keys.S;
      this.mfSaveAs.ShowShortcutKeys = true;
      //
      this.mfExit.Text             = "�I��(&X)";
      this.mfExit.ShortcutKeys     = Keys.Alt | Keys.X;
      this.mfExit.ShowShortcutKeys = true;


      // Menubar
      this.mTool.Dock = DockStyle.Top;
      this.mbtnType.Text = "������";
      //
      this.mtString.Text             = "������";
      this.mtString.ShortcutKeys     = Keys.Control | Keys.D1;
      this.mtString.ShowShortcutKeys = true;
      //
      this.mtIcon.Text               = "�A�C�R��";
      this.mtIcon.ShortcutKeys       = Keys.Control | Keys.D2;
      this.mtIcon.ShowShortcutKeys   = true;
      //
      this.mtImage.Text              = "�摜";
      this.mtImage.ShortcutKeys      = Keys.Control | Keys.D3;
      this.mtImage.ShowShortcutKeys  = true;
      //
#if ENABLE_AUDIO
      this.mtAudio.Text              = "�I�[�f�B�I";
      this.mtAudio.ShortcutKeys      = Keys.Control | Keys.D4;
      this.mtAudio.ShowShortcutKeys  = true;
#endif
      //
      this.mtFile.Text               = "�t�@�C��";
#if ENABLE_AUDIO
      this.mtFile.ShortcutKeys       = Keys.Control | Keys.D5;
#else
      this.mtFile.ShortcutKeys       = Keys.Control | Keys.D4;
#endif
      this.mtFile.ShowShortcutKeys   = true;

      //
      this.mbtnAdd.Text    = "���\�[�X�̒ǉ�(&A)";
      //
      this.mbtnDelete.Text = "���\�[�X�̍폜(&D)";


      // �p�l��
      this.pnlContent.Dock = DockStyle.Fill;
      this.pnlContent.Location = new Point(0, 0);

      // String - ���\�[�X
      this.dgvString.Location = new Point(0, 0);
      this.dgvString.Dock     = DockStyle.Fill;
      this.dgvString.AllowUserToAddRows = true;
      this.dgvString.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      // Icon - ���\�[�X
      this.lvIcon.Location       = new Point(0, 0);
      this.lvIcon.Dock           = DockStyle.Fill;
      this.lvIcon.Visible        = false;
      this.lvIcon.View           = View.LargeIcon;
      this.lvIcon.LargeImageList = new ImageList();
      this.lvIcon.LargeImageList.ImageSize = new Size(160, 120);
      // Image - ���\�[�X
      this.lvImage.Location       = new Point(0, 0);
      this.lvImage.Dock           = DockStyle.Fill;
      this.lvImage.Visible        = false;
      this.lvImage.View           = View.LargeIcon;
      this.lvImage.LargeImageList = new ImageList();
      this.lvImage.LargeImageList.ImageSize = new Size(160, 120);
#if ENABLE_AUDIO
      // Audio - ���\�[�X
      this.lvAudio.Location = new Point(0, 0);
      this.lvAudio.Dock     = DockStyle.Fill;
      this.lvAudio.Visible  = false;
      this.lvAudio.View     = View.List;
#endif
      // File - ���\�[�X
      this.lvFile.Location = new Point(0, 0);
      this.lvFile.Dock     = DockStyle.Fill;
      this.lvFile.Visible  = false;
      this.lvFile.View     = View.List;

      this.dgvString.Columns.Add("Name", "���O");
      this.dgvString.Columns.Add("Value", "�l");


      // Form
      this.Text          = "Resource Generator";
      this.MainMenuStrip = this.mMain;
      this.Size          = new Size(960, 640);

      this.ResumeLayout(false);
      this.PerformLayout();
    }
#endregion
  }
}
