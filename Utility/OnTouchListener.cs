using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

/// <summary>
/// Purpose is to defocus off of textview when clicking outisde the box
/// </summary>
public class ViewClickListener : Java.Lang.Object, View.IOnTouchListener
{
    private Context mContext;
    private EditText mEdittext;

    public ViewClickListener(Context context, EditText edittext)
    {
        mEdittext = edittext;
        mContext = context;
    }

    public bool OnTouch(View v, MotionEvent e)
    {
        InputMethodManager imm = (InputMethodManager)mContext.GetSystemService(Context.InputMethodService);
        imm.HideSoftInputFromWindow(mEdittext.WindowToken, 0);
        return true;
    }
}