// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

namespace CSharpMetal.Util.Clonable
{
    public interface ITCloneable<out T>
    {
        T Clone();
    }
}