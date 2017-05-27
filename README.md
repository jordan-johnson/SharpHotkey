# SharpHotkey

SharpHotkey is a small library to easily add global hotkeys to your Windows Forms application. Simply add the component file `SharpHotkey.dll` to your project references and follow the guide below to get started.

## Usage

```cs
using SharpHotkey;

namespace MyProject {
	public partial class MainForm : Form {
		private Hotkey _hk;

		private void MainForm_Load(object sender, EventArgs e) {
			_hk = new Hotkey(this, Keys.A, Hotkey.Modifiers.Ctrl + Hotkey.Modifiers.Shift);
		}

		protected override void WndProc(ref Message m) {
			if(m.Msg == Hotkey.Message.Id) {
				MessageBox.Show("Pressed!");
			}

			base.WndProc(ref m);
		}
	}
}
```

## Summary

In the usage above, I attached an on-load event to the form then I instantiated the `Hotkey` class by providing the current form, a standard key using the `Keys` enum, and the key modifiers (ctrl and shift). I also implemented `WndProc` to execute an action when the hotkey is used. A `MessageBox` will appear displaying `Pressed!`

The hotkey will automatically deregister itself when the form closes.

## Special Thanks

Thank you to [Curtis Rutland](http://www.dreamincode.net/forums/topic/180436-global-hotkeys/) for explaining in great detail how everything works on the back-end. I made this library to simplify the process and will be using it in my upcoming project, SharpRevise.