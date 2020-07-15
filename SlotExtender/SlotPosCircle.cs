using UnityEngine;

namespace SlotExtender
{
    public class SlotPosCircle : SlotPosLayout
    {
        public override Vector2 VehicleImgPos => new Vector2(col4, -Unit + rowhalf);
        
        private const float row1 = Unit;
        private const float row2 = row1 - RowStep * 3f;
        private const float row3 = row1 - RowStep * 3.97f;
        private const float rowhalf = RowStep * 0.5f;

        private const float col1 = -RowStep * 1.5f;
        private const float col2 = -RowStep * 0.5f;
        private const float col3 = RowStep * 0.5f;
        private const float col4 = RowStep * 1.5f;

        public override Vector2[] SlotPos => new Vector2[12]
        {
            new Vector2(col1, row1 - rowhalf), // slot 1
            new Vector2(col2, row1),           // slot 2
            new Vector2(col3, row1),           // slot 3
            new Vector2(col4, row1 - rowhalf), // slot 4

            new Vector2(col1, row2 + rowhalf), // slot 5
            new Vector2(col2, row2),           // slot 6
            new Vector2(col3, row2),           // slot 7
            new Vector2(col4, row2 + rowhalf), // slot 8

            new Vector2(col1, row3), // slot 9
            new Vector2(col2, row3), // slot 10
            new Vector2(col3, row3), // slot 11
            new Vector2(col4, row3)  // slot 12
        };

        public override Vector2[] ArmSlotPos => new Vector2[2]
        {
            new Vector2(col4, -Unit * 0.1f), // arm slot left
            new Vector2(col1, -Unit * 0.1f)  // arm slot right
        };        
    }
}
