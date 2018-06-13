using System;

namespace Roguelike
{
    class Weapon : Item
    {

        public int AttackPower { get; private set; }
        public double Durability { get; private set; }
        public TypesOfWeapon WeaponType { get; set; }
        public Position WeaponPos { get; set; }
        Random rnd = new Random();

        public Weapon(Position pos)
        {
            WeaponType = (TypesOfWeapon)rnd.Next(0, Enum.GetNames(typeof(TypesOfTraps)).Length);
            WeaponPos = pos;
        }

        public Weapon(TypesOfWeapon typeweapon)
        {
            WeaponType = typeweapon;
            SetDurAndAP();
        }

        public void SetDurAndAP()
        {
            switch (WeaponType)
            {
                case TypesOfWeapon.Axe:
                    AttackPower = rnd.Next(5, 15);
                    Durability = rnd.NextDouble();
                    break;
                case TypesOfWeapon.Dagger:
                    AttackPower = rnd.Next(1, 10);
                    Durability = rnd.NextDouble();
                    break;
                case TypesOfWeapon.Lance:
                    AttackPower = rnd.Next(5, 20);
                    Durability = rnd.NextDouble();
                    break;
                case TypesOfWeapon.SlingShot:
                    AttackPower = rnd.Next(1, 5);
                    Durability = rnd.NextDouble();
                    break;
                case TypesOfWeapon.Sword:
                    AttackPower = rnd.Next(5, 20);
                    Durability = rnd.NextDouble();
                    break;
            }
        }

        public override string ToString()
        {

            return ($"{WeaponType,-14}| {AttackPower,14} | {Durability,14}");
        }
    }
}
