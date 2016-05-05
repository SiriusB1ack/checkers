
namespace BionicFinalProject.Model
{

    /// <sumary>
    ///  Used to represent a player game move
    /// </sumary>
    public class Move
    {
        // Departure board house
        public int from { get; set; }

        // Target board house
        public int to { get; set; }



        public Move(int moveFrom, int moveTo)
        {
            from = moveFrom;
            to = moveTo;
        }




        /// <sumary>
        ///  Returns a string representation of the class
        /// </sumary>
        public override string ToString()
        {
            return "(" + from + "," + to + ")";
        }
    }
}
