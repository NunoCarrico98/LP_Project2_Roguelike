using System;

namespace Roguelike
{
    [Serializable()]
    public class Weapon : Item
    {

        public int AttackPower { get; private set; }
        public double Durability { get; private set; }
        public TypesOfWeapon WeaponType { get; set; }
        private Random rnd = new Random(Guid.NewGuid().GetHashCode());

        public Weapon()
        {
            WeaponType = (TypesOfWeapon)
                rnd.Next(Enum.GetNames(typeof(TypesOfWeapon)).Length);
            SetDurAndAP();
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
                    AttackPower = 15;
                    Durability = 0.85d;
                    Weight = 5f;
                    break;
                case TypesOfWeapon.Dagger:
                    AttackPower = 5;
                    Durability = 0.9d;
                    Weight = 2f;
                    break;
                case TypesOfWeapon.Lance:
                    AttackPower = 25;
                    Durability = 0.75d;
                    Weight = 9f;
                    break;
                case TypesOfWeapon.SlingShot:
                    AttackPower = 3;
                    Durability = 0.95;
                    Weight = 1f;
                    break;
                case TypesOfWeapon.Sword:
                    AttackPower = 20;
                    Durability = 0.8d;
                    Weight = 7f;
                    break;
            }
        }

        public override string ToString()
        {

            return ($"{WeaponType,-14}| {AttackPower,13} | {Weight,10} | " +
                $"{Durability,12:f1}|");
        }
    }
}
