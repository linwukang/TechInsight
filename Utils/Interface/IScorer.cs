namespace Utils.Interface;

public interface IScorer<in TElement>
{
    double Score(TElement obj);

    int Compare(TElement objA, TElement objB)
    {
        var scoreA = Score(objA);
        var scoreB = Score(objB);

        if (scoreA == scoreB)
        {
            return 0;
        }
        else if (scoreA > scoreB)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}