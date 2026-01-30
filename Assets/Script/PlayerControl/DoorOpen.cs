public class DoorOpen: BaseInteractable
{
   protected override void OnInteract()
   {
      //绕y轴转-90
      transform.Rotate(0, -135, 0);
   }
}