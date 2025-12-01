{$reference System.Drawing.dll}
{$reference System.Windows.Forms.dll}
{$reference TaskbarProgress.dll}


uses System;
uses System.Diagnostics;
uses System.Drawing;
uses System.Windows.Forms;
uses TaskbarProgress;


type
  MainForm = class(Form)
    private _TestButton : Button;
  
    public constructor ();
    begin
      _TestButton          := new Button();
      _TestButton.Size     := new System.Drawing.Size(75, 23);
      _TestButton.Location := new Point(20, 20);
      _TestButton.Text     := 'Test';
      _TestButton.Click    += TestButtonClick;
      Controls.Add(_TestButton);
    end;
    
    private procedure TestButtonClick(sender: object; e: EventArgs);
    begin
      Taskbar.ProgressInit();
      Taskbar.ProgressState := TaskbarProgressState.Normal;
      Taskbar.ProgressValue := 50;
    end;
  end;

begin
  Application.SetCompatibleTextRenderingDefault(false);
  Application.EnableVisualStyles();
  Application.Run(new MainForm());
end.