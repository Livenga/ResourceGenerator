using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;

using Liven.Data;


namespace Liven.Forms {
  public partial class ResGenForm : Form {
    /// <summary></summary>
#region public ResourceMode ResourceMode { private set; get; }
    public ResourceMode ResourceMode {
      private set {
        if(this.mode == value) { return; }

        this.mode = value;

        // UI の遷移
        foreach(Control c in this.pnlContent.Controls) {
          if(c is ListView || c is DataGridView) {
            c.Visible = false;
          }
        } // foreach(Control)

        switch(value) {
          case ResourceMode.String:
            this.mbtnType.Text     = "文字列";
            this.dgvString.Visible = true;
            break;
          case ResourceMode.Icon:
            this.mbtnType.Text  = "アイコン";
            this.lvIcon.Visible = true;
            break;
          case ResourceMode.Image:
            this.mbtnType.Text   = "画像";
            this.lvImage.Visible = true;
            break;
#if ENABLE_AUDIO
          case ResourceMode.Audio:
            this.mbtnType.Text   = "オーディオ";
            this.lvAudio.Visible = true;
            break;
#endif
          case ResourceMode.File:
            this.mbtnType.Text  = "ファイル";
            this.lvFile.Visible = true;
            break;
        }
      }
      get {
        return this.mode;
      }
    }
#endregion

    /// <summary></summary>
#region public string ResourcePath { private set; get }
    public string ResourcePath {
      private set {
        this.resourcePath = value;

        if(value == null) {
          this.Text = "Resource Generator";
        } else {
          this.Text = value + " - " + "Resource Generator";
        }
      }
      get {
        return this.resourcePath;
      }
    }
#endregion

    private ResourceMode mode;
    // 事前に開かれているリソースのパス
    private string resourcePath = null;


    /// <summary></summary>
#region public ResGenForm()
    public ResGenForm() {
      this.InitializeComponent();

      this.ResourceMode = ResourceMode.String;

      this.Shown += new EventHandler(this.onShownForm);
      this.FormClosing += new FormClosingEventHandler(this.onFormClosing);

      this.mbtnType.DropDownItemClicked +=
        new ToolStripItemClickedEventHandler(this.onClickDropDownItem);

      //
      this.mfOpen.Click   += new EventHandler(this.onClickOpen);
      this.mfClose.Click  += new EventHandler(this.onClickClose);
      this.mfSave.Click   += new EventHandler(this.onClickSave);
      this.mfSaveAs.Click += new EventHandler(this.onClickSave);
      this.mfExit.Click   += new EventHandler(this.onClickExitMenu);

      this.mbtnAdd.Click    += new EventHandler(this.onClickAddResource);
      this.mbtnDelete.Click += new EventHandler(this.onClickDeleteResource);
    }
#endregion


    //
    // private
    //

    // Bitmap の作成
#region private Bitmap CreateBitmap(string, bool)
    private Bitmap CreateBitmap(string path, bool isThumbnail) {
      Bitmap bmp;

      try {
        using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
          if(this.ResourceMode == ResourceMode.Image) {
            bmp = new Bitmap(Image.FromStream(fs));
            // Image
          } else {
            // Icon
            bmp = new Icon(fs).ToBitmap();
          }


          // Thumbnail の作成
          if(isThumbnail) {
            Bitmap thumb;
            int    width, height;

            if(bmp.Width > bmp.Height) {
              width  = 320;
              height = (int)(bmp.Height * ((double)320.0 / bmp.Width));
            } else {
              height = 240;
              width  = (int)(bmp.Width * ((double)240.0 / bmp.Height));
            }

            thumb = new Bitmap(bmp.GetThumbnailImage(width, height,
                new Image.GetThumbnailImageAbort(() => { return false; }),
                IntPtr.Zero));

            bmp.Dispose();
            bmp = thumb;
          }

          return bmp;
        }
      } catch(Exception except) {
#if DEBUG
        Console.Error.WriteLine(except.GetType().ToString());
        Console.Error.WriteLine(except.Message);
        Console.Error.WriteLine(except.StackTrace);
#endif
        return null;
      }
    }
#endregion

    // ファイルパスを指定して byte[] を取得
#region private byte[] FileToBytes(string)
    private byte[] FileToBytes(string path) {
      byte[] bytes;

      using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
        bytes = new byte[fs.Length];

        using(BinaryReader br = new BinaryReader(fs)) {
          br.Read(bytes, 0, (int)fs.Length);
        }
      }

      return bytes;
    }
#endregion

    //
#region private void ClearResources()
    private void ClearResources() {
      this.dgvString.Rows.Clear();

      this.lvIcon.LargeImageList.Images.Clear();
      this.lvIcon.Items.Clear();
      
      this.lvImage.LargeImageList.Images.Clear();
      this.lvImage.Items.Clear();

      this.lvFile.Items.Clear();
    }
#endregion


    //
    // イベント
    //

    //
#region private void onShownForm(object, EventArgs)
    private void onShownForm(object sender, EventArgs e) {
    }
#endregion

    //
#region private void onFormClosing(object, FormClosingEventArgs)
    private void onFormClosing(object sender, FormClosingEventArgs e) {
    }
#endregion

    //
    // メニューバー
    //

    //
#region private void onClickDropDownItem(object, ToolStripItemClickedEventArgs)
    private void onClickDropDownItem(object sender, ToolStripItemClickedEventArgs e) {
      ToolStripMenuItem item;

      if(e.ClickedItem is ToolStripMenuItem) {
        item = e.ClickedItem as ToolStripMenuItem;
        if(item.Equals(this.mtString))     this.ResourceMode = ResourceMode.String;
        else if(item.Equals(this.mtIcon))  this.ResourceMode = ResourceMode.Icon;
        else if(item.Equals(this.mtImage)) this.ResourceMode = ResourceMode.Image;
#if ENABLE_AUDIO
        else if(item.Equals(this.mtAudio)) this.ResourceMode = ResourceMode.Audio;
#endif
        else if(item.Equals(this.mtFile))  this.ResourceMode = ResourceMode.File;
      }
    }
#endregion

    //
#region private void onClickAddResource(object, EventArgs)
    private void onClickAddResource(object sender, EventArgs e) {
      ListView lv = null;

      // 文字列のリソースは, ファイルを選択しないため
      // DataGridView の最後の位置にフォーカスを当てる.
      if(this.ResourceMode == ResourceMode.String) {
        this.dgvString.CurrentCell =
          this.dgvString.Rows[this.dgvString.NewRowIndex].Cells[0];
        return;
      }

      OpenFileDialog dialog;

      dialog = new OpenFileDialog();
      dialog.Title            = "リソースファイルの選択";
      dialog.Multiselect      = true;
      dialog.InitialDirectory = Directory.GetCurrentDirectory();
      //dialog.RestoreDirectory = true;

      switch(this.ResourceMode) {
        case ResourceMode.Icon:
          lv = this.lvIcon;
          dialog.Filter =
            "Icon File(*.ico)|*.ico|All File(*.*)|*.*";
          break;
        case ResourceMode.Image:
          lv = this.lvImage;
          dialog.Filter =
            "Image File(*.jpg;*.png;*.bmp;*.gif)|*.jpg;*.png;*.bmp;*.gif|All File(*.*)|*.*";
          break;
#if ENABLE_AUDIO
        case ResourceMode.Audio:
          lv = this.lvAudio;
          dialog.Filter =
            "Audio File(*.mp3;*.wmv;*.wav)|*.mp3;*.wmv;*.wav|All File(*.*)|*.*";
          break;
#endif
        case ResourceMode.File:
          lv = this.lvFile;
          dialog.Filter =
            "All File(*.*)|*.*";
          break;
      }
      dialog.FilterIndex = 0;

      if(dialog.ShowDialog() == DialogResult.OK) {
        foreach(string fileName in dialog.FileNames) {
          ListViewItem item;

          item = new ListViewItem(System.IO.Path.GetFileName(fileName));
          item.Name = fileName;

          if(this.ResourceMode == ResourceMode.Image
              || this.ResourceMode == ResourceMode.Icon) {
            Bitmap bitmap = this.CreateBitmap(fileName, false);

            lv.LargeImageList.Images.Add(bitmap);
            item.ImageIndex = lv.LargeImageList.Images.Count - 1;
          }

          lv.Items.Add(item);
        }
      }

      dialog.Dispose(); dialog = null;
      System.GC.Collect();
    }
#endregion

    // 選択されたリソースの削除
#region private void onClickDeleteResource(object, EventArgs)
    private void onClickDeleteResource(object sender, EventArgs e) {
      ListView lv;

      if(this.ResourceMode == ResourceMode.String) {
        foreach(DataGridViewRow row in this.dgvString.SelectedRows) {
          // 新規追加の行以外
          if(row.IsNewRow == false) {
            this.dgvString.Rows.Remove(row);
          }
        }
      } else if(this.ResourceMode == ResourceMode.Image
          || this.ResourceMode == ResourceMode.Icon) {
        if(this.ResourceMode == ResourceMode.Image) {
          lv = this.lvImage;
        } else {
          lv = this.lvIcon;
        }

        int nofSelectedItems = lv.SelectedItems.Count;
        int[] idxes = new int[lv.SelectedItems.Count];

        // Index の取得
        for(int i = 0; i < lv.SelectedItems.Count; ++i) {
          idxes[i] = lv.SelectedItems[i].Index;
        }

        // 選択された要素を削除
        for(int i = idxes.Length - 1; i >= 0; --i) {
          lv.Items.RemoveAt(idxes[i]);
          lv.LargeImageList.Images.RemoveAt(idxes[i]);
        }
        // ImageList の番号を調整
        for(int i = 0; i < lv.Items.Count; ++i) {
          lv.Items[i].ImageIndex = i;
        }

        idxes = null;
      } else {
#if ENABLE_AUDIO
        if(this.ResourceMode == ResourceMode.Audio) {
          lv = this.lvAudio;
        } else {
          lv = this.lvFile;
        }
#else
        lv = this.lvFile;
#endif

        foreach(ListViewItem lvItem in lv.SelectedItems) {
          lv.Items.Remove(lvItem);
        }
      }
    }
#endregion

    //
    // メニュー
    //

    // 開く
#region private void onClickOpen(object, EventArgs)
    private void onClickOpen(object sender, EventArgs e) {
      OpenFileDialog dialog;
#if DEBUG
      Console.Error.WriteLine("* onClickOpen");
#endif

      dialog = new OpenFileDialog();
      dialog.InitialDirectory = Directory.GetCurrentDirectory();
      dialog.Filter           = "Resource File(*.resources)|*.resources|All Files(*.*)|*.*";
      dialog.FilterIndex      = 0;
      dialog.Multiselect      = false;

      if(dialog.ShowDialog() == DialogResult.OK) {
        string path = dialog.FileName;

        if(File.Exists(path)) {
          this.ResourcePath    = path;
          this.mfClose.Enabled = true;

          // 表示中のリソースを破棄
          this.ClearResources();

          using(ResourceReader reader = new ResourceReader(path)) {
            foreach(DictionaryEntry entry in reader) {
              string key = entry.Key as string;
              object o   = entry.Value;

              Console.WriteLine("{0}: {1}", key, o.GetType().ToString());
              if(o is string) {
                // 文字列
                DataGridViewRow row = new DataGridViewRow();
                row.Cells.AddRange(new DataGridViewCell[] {
                    new DataGridViewTextBoxCell() { Value = key },
                    new DataGridViewTextBoxCell() { Value = o }
                    });

                this.dgvString.Rows.Add(row);
              } else if(o is Icon) {
                // アイコン
                ListViewItem item = new ListViewItem(key);
                Bitmap bmp = (o as Icon).ToBitmap();

                item.Tag = o;
                item.ImageIndex = this.lvIcon.Items.Count;

                this.lvIcon.Items.Add(item);
                this.lvIcon.LargeImageList.Images.Add(
                    bmp.GetThumbnailImage(320, 120,
                      new Image.GetThumbnailImageAbort(() => { return false; }),
                      IntPtr.Zero));
              } else if(o is Bitmap) {
                // 画像
                ListViewItem item = new ListViewItem(key);
                Bitmap bmp = o as Bitmap;

                item.Tag        = o;
                item.ImageIndex = this.lvImage.Items.Count;

                this.lvImage.Items.Add(item);
                this.lvImage.LargeImageList.Images.Add(
                    bmp.GetThumbnailImage(320, 120,
                    new Image.GetThumbnailImageAbort(() => { return false; }),
                    IntPtr.Zero));
              } else {
                // ファイル
                this.lvFile.Items.Add(new ListViewItem(key) { Tag = (object)o });
              }
            }
          }
        } else {
        }
      }

      dialog.Dispose(); dialog = null;
      System.GC.Collect();
    }
#endregion

    // 閉じる
#region private void onClickClose(object sender, EventArgs e)
    private void onClickClose(object sender, EventArgs e) {
      this.ResourcePath = null;
      this.ClearResources();

      (sender as ToolStripMenuItem).Enabled = false;
    }
#endregion

    // 保存
#region private void onClickSave(object, EventArgs)
    private void onClickSave(object sender, EventArgs e) {
      string resourcePath = null;
      bool isSaveAs = (sender as ToolStripMenuItem).Equals(this.mfSaveAs);


      if(isSaveAs || this.ResourcePath == null) {
        // 名前を付けて保存
        SaveFileDialog dialog = new SaveFileDialog();

        dialog.Filter           = "Resource File(*.resources)|*.resources|All File(*.*)|*.*";
        dialog.FilterIndex      = 0;
        //dialog.RestoreDirectory = true;
        dialog.InitialDirectory = Directory.GetCurrentDirectory();

        if(dialog.ShowDialog() == DialogResult.OK) {
          resourcePath = dialog.FileName;
        } else {
          return;
        }
        dialog.Dispose(); dialog = null;
      } else {
        // 上書き
        resourcePath = this.resourcePath;
      }

      // 未指定
      if(resourcePath == null) {
        return;
      }


      // 書き込み処理
      bool isOverride = File.Exists(resourcePath);
      if(isOverride) {
        File.Delete(resourcePath);
      }

      Console.WriteLine(resourcePath);
      using(FileStream fs = new FileStream(resourcePath,
            FileMode.OpenOrCreate, FileAccess.Write)) {
        using(ResourceWriter rw = new ResourceWriter(fs)) {
          // 文字列 - String
          foreach(DataGridViewRow row in this.dgvString.Rows) {
            if(row.IsNewRow == false && row.Cells[0].Value != null) {
#if DEBUG
              Console.WriteLine("Key[{0}] {1}",
                  row.Cells[0].Value as string,
                  row.Cells[1].Value as string);
#endif
              rw.AddResource(row.Cells[0].Value as string, row.Cells[1].Value as string);
            }
          } // foreach(DataGridViewRow)

          string name, path;
          // アイコン - Icon
          foreach(ListViewItem item in this.lvIcon.Items) {
            Icon icon;

            name = item.SubItems[0].Text;
            path = item.SubItems[0].Name;

            if(item.Tag != null) {
              Console.WriteLine(item.Tag.GetType().ToString());
              icon = item.Tag as Icon;
            } else {
              icon = new Icon(path);
            }

            rw.AddResource(name, icon);
            //icon.Dispose(); icon = null;
          }
          // 画像 - Image
          foreach(ListViewItem item in this.lvImage.Items) {
            Bitmap bmp;

            name = item.SubItems[0].Text;
            path = item.SubItems[0].Name;

            if(item.Tag != null) {
              bmp = item.Tag as Bitmap;
            } else {
              Console.WriteLine(path);

              using(FileStream fsImage =
                  new FileStream(path, FileMode.Open, FileAccess.Read)) {
                bmp  = new Bitmap(Image.FromStream(fsImage));
              }
            }

            rw.AddResource(name, bmp);
            //bmp.Dispose(); bmp = null;
          }

#if ENABLE_AUDIO
          // TODO: Audio 関係のクラスが存在するかも
          // オーディオ - Audio
          foreach(ListViewItem item in this.lvAudio.Items) {
            name = item.SubItems[0].Text;
            path = item.SubItems[0].Name;
            rw.AddResource(name, this.FileToBytes(path));
          }
#endif
          // ファイル - File
          foreach(ListViewItem item in this.lvFile.Items) {
            name = item.SubItems[0].Text;
            path = item.SubItems[0].Name;

            Console.WriteLine("{0} : {1}", name, path);
            if(item.Tag == null) {
              rw.AddResource(name, this.FileToBytes(path));
            } else {
              Console.WriteLine(item.Tag.GetType().ToString());
              rw.AddResource(name, item.Tag);
            }
          }
        }
      }
    }
#endregion

    //
#region private void onClickExitMenu(object, EventArgs)
    private void onClickExitMenu(object sender, EventArgs e) {
      this.Close();
    }
#endregion

  }
}
