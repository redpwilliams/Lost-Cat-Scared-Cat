public class WhiteFox : Fox
{

  protected override void Attack()
  {
    this.hasAttacked = true;
    this.HasInputJump = true;
  }

}