using UnityEngine;

public class Controls {
    private static KeyCode GO_FORWARD = KeyCode.W;
    private static KeyCode GO_BACKWARD = KeyCode.S;
    private static KeyCode ROTATE_LEFT = KeyCode.A;
    private static KeyCode ROTATE_RIGHT = KeyCode.D;
    private static KeyCode RUNNING = KeyCode.LeftShift;
    private static KeyCode PICK_UP_OBJ = KeyCode.F;
    private static KeyCode USE_OBJ = KeyCode.E;
    private static KeyCode DROP_OBJ = KeyCode.G;
    private static KeyCode TOGGLE_HINT = KeyCode.H;
    private static KeyCode NEXT_HINT = KeyCode.RightArrow;
    private static KeyCode PREV_HINT = KeyCode.LeftArrow;
    private static KeyCode RESET_PLAYER = KeyCode.R;
    private static KeyCode TOGGLE_MENU = KeyCode.Escape;

    public static KeyCode GoForward {get {return GO_FORWARD; } }
    public static KeyCode GoBackward {get {return GO_BACKWARD; } }
    public static KeyCode RotateLeft {get {return ROTATE_LEFT; } }
    public static KeyCode RotateRight {get {return ROTATE_RIGHT; } }
    public static KeyCode Running {get {return RUNNING; } }
    public static KeyCode PickUpObj {get {return PICK_UP_OBJ; } }
    public static KeyCode UseObj {get {return USE_OBJ; } }
    public static KeyCode DropObj {get {return DROP_OBJ; } }
    public static KeyCode ToggleHint {get {return TOGGLE_HINT; } }
    public static KeyCode NextHint {get {return NEXT_HINT; } }
    public static KeyCode PrevHint {get {return PREV_HINT; } }
    public static KeyCode ResetPlayer {get {return RESET_PLAYER; } }
    public static KeyCode ToggleMenu {get {return TOGGLE_MENU; } }
}