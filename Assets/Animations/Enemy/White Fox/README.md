# White Fox Animation Controller and Animations

## Animation Controller

<img src="./Layer.png">

- From any state, the White Fox should be able to jump and fall. This is to handle future cases in which circumstances get more specific.
  - With that, notice how no transitions are being done to the Jump or Fall animations except the one from Any State.
- Everything else relies on three, bool parameters:
  1. `IsRunning`
  2. `IsJumping`
  3. `IsFalling`
  - Each transition uses each bool value. As of this commit, it works well.
  - This is set through code:

```c#
  private static readonly int Running = Animator.StringToHash("IsRunning");
  private static readonly int Jumping = Animator.StringToHash("IsJumping");
  private static readonly int Falling = Animator.StringToHash("IsFalling");
```

- This will/should later be used in the Character class as will apply to both the Player and all Fox instances.
