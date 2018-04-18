using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Outsourcing_System;


public class CompareFiles
{
    public ICompFile firstFile;
    public ICompFile secondFile;

    private int list1PrevMatchIndex;
    private int list2PrevMatchIndex;
    private MissedMatch currMisMatch;

    public MissedMatch CurrentMisMatch
    {
        get
        {
            return this.currMisMatch;
        }
    }

    ArrayList misMatchList = new ArrayList();

    /// <summary>
    /// Starts matching from the provided indeces in respective lists and returns when a mismatch or end of lists is reached
    /// </summary>
    /// <param name="list1StartIndex"></param>
    /// <param name="list2StartIndex"></param>
    private int NextMisMatch(int list1StartIndex, int list2StartIndex)
    {
        ArrayList firstList = firstFile.GenerateAndGetAllWordsInFile();
        ArrayList secondList = secondFile.GenerateAndGetAllWordsInFile();
        if (firstList.Count > 0 && secondList.Count > 0)
        {
            if (list1PrevMatchIndex != -1)
            {
                //TODO:chk for any previous index selected in a listbox here
                if (list1PrevMatchIndex > list1StartIndex || list2PrevMatchIndex > list2StartIndex)
                {
                    //lblMsg.Text = "Cannot select previous elements, please reselect";
                    //this.listBox1.SelectedIndex = list1PrevMatchIndex;
                    //this.listBox2.SelectedIndex = list2PrevMatchIndex;
                    this.currMisMatch = null;
                    return -1;
                }
                else
                {
                    //AddMisMatchItem(list1StartIndex, list2StartIndex);
                    return -1;
                }
            }
            int i = list1StartIndex, j = list2StartIndex;
            string list1Item = "", list2Item = "";
            do
            {
                list1Item = ((Word)firstList[i]).Text;
                list2Item = ((Word)secondList[j]).Text;
                if (list1Item != list2Item)
                {
                    //listBox1.SelectedIndex = i;
                    //listBox2.SelectedIndex = j;
                    list1PrevMatchIndex = i;
                    list2PrevMatchIndex = j;
                    //lblMsg.Text = "Possible Mismatch, please select a Match and continue";
                    this.currMisMatch = new MissedMatch(i, j);
                    return 2;
                }
                if (i <= firstList.Count - 2)
                {
                    i++;
                }
                else
                {
                    this.currMisMatch = null;
                    return 1; //end of list1 reached
                }
                if (j <= secondList.Count - 2)
                    j++;
                else
                {
                    this.currMisMatch = null;
                    return 1;// end of list2 reached
                }

            } while (true);
        }
        return -1;
    }

    /// <summary>
    /// Starts from the begining
    /// </summary>
    public int NextMisMatch()
    {
        return NextMisMatch(0, 0);
    }
}
public class MissedMatch
{
    public int list1Index;
    public int list2Index;
    //public ArrayList list1Items;
    //public ArrayList list2Items;
    //public int list1MMLen;
    //public int list2MMLen;
    //public string comments;
    //public MisMatchType misMatchType;
    public MissedMatch(int List1Index, int List2Index)
    {
        list1Index = List1Index;
        list2Index = List2Index;
        //list1MMLen = -1;
        //list2MMLen = -1;
        //comments = "";
        //misMatchType = new MisMatchType();
    }

}

