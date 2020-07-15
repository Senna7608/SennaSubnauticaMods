using UnityEngine;

namespace SlotExtender
{
    public class SlotPosGrid : SlotPosLayout
    {
        public override Vector2 VehicleImgPos => SlotPos[11];
                
        private const float TopRow = Unit;
        private const float SecondRow = TopRow - RowStep;
        private const float ThirdRow = SecondRow - RowStep;
        private const float FourthRow = ThirdRow - RowStep;
        private const float FifthRow = FourthRow - RowStep;
        private const float CenterColumn = 0f;
        private const float RightColumn = RowStep;
        private const float LeftColumn = -RowStep;

        public override Vector2[] SlotPos => new Vector2[12]
        {
            new Vector2(LeftColumn, TopRow),       // slot 1
            new Vector2(CenterColumn, TopRow),     // slot 2
            new Vector2(RightColumn, TopRow),      // slot 3

            new Vector2(LeftColumn, SecondRow),    // slot 4
            new Vector2(CenterColumn, SecondRow),  // slot 5
            new Vector2(RightColumn, SecondRow),   // slot 6

            new Vector2(LeftColumn, ThirdRow),     // slot 7
            new Vector2(CenterColumn, ThirdRow),   // slot 8
            new Vector2(RightColumn, ThirdRow),    // slot 9

            new Vector2(LeftColumn, FourthRow),    // slot 10
            new Vector2(CenterColumn, FourthRow),  // slot 11
            new Vector2(RightColumn, FourthRow)    // slot 12
        };

        public override Vector2[] ArmSlotPos => new Vector2[2]
        {
            new Vector2(LeftColumn, FifthRow),     // arm slot left
            new Vector2(RightColumn, FifthRow)     // arm slot right
        };        
    }
}
