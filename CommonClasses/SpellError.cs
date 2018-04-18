using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


   public class SpellError
    {
       public SpellError()
       { 
       }
       private string word;

       public string Word
       {
           get { return word; }
           set { word = value; }
       }
       private int occurences;

       public int Occurences
       {
           get { return occurences; }
           set { occurences = value; }
       }
       private string pageNo;

       public string PageNo
       {
           get { return pageNo; }
           set { pageNo = value; }
       }
       private string regex;

       public string Regex
       {
           get { return regex; }
           set { regex = value; }
       }

    }

