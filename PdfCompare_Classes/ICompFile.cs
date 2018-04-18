using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;


public interface ICompFile
{
    string FilePath { get; set; }
    int TotalPages { get; }
    int WordCount { get; }
    ArrayList WordsArrayList { get; }
    //List<Word> GetAllWordsInFile();
    ArrayList GenerateAndGetAllWordsInFile();
    string GetPageLines(int PageNumber, string linedelimeterString);
}

