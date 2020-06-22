using Android.App;
using Android.Gms.Maps;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Anxityy.Fragments
{
    public class Single_RecordAnx : Android.Support.V4.App.Fragment, IOnMapReadyCallback
    {
        private GoogleMap mMap;
        private AnxityRecords anx;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.singleRecordAnx, container, false);
            LinearLayout anxLayout = view.FindViewById<LinearLayout>(Resource.Id.anxLayout);

            SetupMap();
            getAnxRecord();
            RelativeLayout formLayout = new RelativeLayout(Activity);
            RelativeLayout.LayoutParams lin1_params = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
            formLayout.LayoutParameters = lin1_params;
            formLayout = createAnxLayout();
            anxLayout.AddView(createAnxLayout());

            return view;
        }
        private void SetupMap()
        {
            if(mMap == null)
            {
                FragmentManager.FindFragmentById(Resource.Id.map);
            }

        }
       public RelativeLayout createAnxLayout()
        {
            RelativeLayout layout_lin1= new RelativeLayout(Activity);
            RelativeLayout.LayoutParams lin1_params = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
            RelativeLayout.LayoutParams lin1_params_left= new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
            RelativeLayout.LayoutParams lin1_params_right = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
      
            RelativeLayout layout_lin1_left = new RelativeLayout(Context);
            RelativeLayout layout_lin1_right= new RelativeLayout(Context);

            layout_lin1.LayoutParameters = lin1_params;
            layout_lin1_left.LayoutParameters = lin1_params_left;
            layout_lin1_right.LayoutParameters = lin1_params_right;
            lin1_params_right.AddRule(LayoutRules.AlignParentRight);
            lin1_params_left.AddRule(LayoutRules.AlignParentLeft);

            TextView textView = new TextView(Activity);
            textView.Text = "Date: " ;
            textView.SetTypeface(Typeface.Serif, TypefaceStyle.Bold);
            textView.SetPadding(60, 50, 0, 50);
            layout_lin1_left.AddView(textView);
            TextView textView2 = new TextView(Activity);
            textView2.Text = "" + anx.date;
            textView2.SetTypeface(Typeface.Serif, TypefaceStyle.Bold);
            textView2.SetPadding(0, 50, 50, 50);
            layout_lin1_right.AddView(textView2);
            layout_lin1.AddView(layout_lin1_left);
            layout_lin1.AddView(layout_lin1_right);
            
            return layout_lin1;
        }
        public void getAnxRecord()
        {

            anx = new AnxityDatabase().GetAnxityRecordAsync(Arguments.GetInt("recordId")).Result;
        }
        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;
        }
       
    }
}