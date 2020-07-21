using Android.Locations;
using System;
using System.Collections.Generic;


public class Prediction
{
    public string description { get; set; }
    public string id { get; set; }
    public List<MatchedSubstring> matched_substrings { get; set; }
    public string place_id { get; set; }
    public string reference { get; set; }
    public List<Term> terms { get; set; }
    public List<string> types { get; set; }
    public Geometry geometry { get; set; }

}

public class GoogleMapPlaceClass
{
    public List<Prediction> predictions { get; set; }
    public string status { get; set; }
}
public class GoogleMapPlaceDetails
{
    public LocationDetails result { get; set; }
    public string status { get; set; }
}
public class LocationDetails
{
    public string name { get; set; }
    public Geometry geometry { get; set; }
}
public class MatchedSubstring
{
    public int length { get; set; }
    public int offset { get; set; }
}
public class Geometry
{
    public Loca location { get; set; }

}
public class Loca
{
    public string lat { get; set; }
    public string lng { get; set; }
}
public class Term
{
    public int offset { get; set; }
    public string value { get; set; }
}


