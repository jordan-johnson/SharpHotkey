using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SharpHotkey {
	public class Hotkey : IDisposable {
		/// <summary>
		/// Window handle (from form)
		/// </summary>
		private IntPtr _hWnd;

		/// <summary>
		/// Collection of modifiers (shift, alt, or ctrl)
		/// </summary>
		private int _modifier;

		/// <summary>
		/// Unique key id
		/// </summary>
		private int _id;

		/// <summary>
		/// Virtual key code
		/// </summary>
		private int _key;

		/// <summary>
		/// Imports hotkey registration method from user32.dll
		/// <para>user32.dll is included with Windows.Forms</para>
		/// </summary>
		/// <param name="hWnd">Window handle (in this case, form)</param>
		/// <param name="id">Key identifier</param>
		/// <param name="fsModifiers">Shift, Alt, Ctrl, etc. modifiers</param>
		/// <param name="vk">Virtual key code</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

		/// <summary>
		/// Imports hotkey deregistration method from user32.dll
		/// <para>user32.dll is included with Windows.Forms</para>
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		/// <summary>
		/// Assigns a global hotkey
		/// </summary>
		/// <param name="form"></param>
		/// <param name="key">Use Keys enum to assign</param>
		/// <param name="modifier">Use Hotkey.Modifiers to assign</param>
		public Hotkey(Form form, Keys key, int modifier) {
			_hWnd = form.Handle;
			_key = (int)key;
			_modifier = modifier;
			_id = this.GetHashCode();

			// if registration unsuccessful
			if(!Register()) {
				throw new Exception("Global hotkey could not be registered");
			}
		}

		/// <summary>
		/// Deconstrucutor attempts to deregister hotkey
		/// </summary>
		~Hotkey() {
			Deregister();
		}

		/// <summary>
		/// Deregister our hotkey
		/// </summary>
		public void Dispose() {
			Deregister();

			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Used in checking if hotkey is registered
		/// </summary>
		public bool IsRegistered = false;

		/// <summary>
		/// Modifiers for hotkey
		/// <para>Available: Alt, Ctrl, Shift</para>
		/// </summary>
		public static class Modifiers {
			public const int Alt = 0x0001;
			public const int Ctrl = 0x0002;
			public const int Shift = 0x0004;
		}

		/// <summary>
		/// In your main form, override WndProc(ref Message m) and check if
		/// the message matches the id. If matched, execute your action.
		/// </summary>
		public static class Message {
			public const int Id = 0x0312;
		}

		/// <summary>
		/// Registers global hotkey
		/// <para>This method is called within the constructor</para>
		/// </summary>
		/// <returns>True if hotkey is registered</returns>
		public bool Register() {
			IsRegistered = RegisterHotKey(_hWnd, _id, _modifier, _key);

			return IsRegistered;
		}

		/// <summary>
		/// Deregisters global hotkey
		/// <para>Run this method in your form closing event</para>
		/// </summary>
		/// <returns>True if hotkey is deregistered</returns>
		public bool Deregister() {
			if(!IsRegistered) return true;

			bool success = UnregisterHotKey(_hWnd, _id);

			IsRegistered = (success) ? false : true;

			if(!success) {
				throw new Exception("Global hotkey could not be deregistered");
			}

			return success;
		}

		/// <summary>
		/// Creates the unique id from the combination of modifiers and key
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() {
			return _modifier ^ _key ^ _hWnd.ToInt32();
		}
	}
}
