# PlayerCustomizationScreen

Thanks for checking out the project!

> This is not player customisation framework.
> It's a framework for basic camera and player rotation view inside of player customization screen.
> This framework will remain `Open Source`

## Status and prerequisites

Current status at a glance:
```
Unity version: 2019.2.1f1
Platforms    : Windows/Mac/Linux/Android/IOS
```

## What is it for ?
- Camera would animate to the assigned target when `PositionCamera()` is called.
- Parameters : 
```
- if id is passed, then it will animate to target's position within Parts list.
- if id is not passed, then it will animate to base target's position.
- In every case, base target's rotation is changed either on MouseDrag/Touch or by calling `PositionCamera()`.
```
- Overload :
```
- This is only made for previewing in editor, if passed first parameter as true and second parameter 
as per above logic for id, the camera would snap to the position depending on id passed or not.
```

## CameraCustomization.cs

![Image](https://github.com/mohitsethi32/PlayerCustomizationScreen/blob/master/Documentation/CustomizationCameraCS.png)

## How to use ?
- Download the project and open Sample Scene inside Scenes folder.
- CameraCustomization.cs would be configured like above on MainCamera (Placing script on camera is mandatory for animating camera).
- The UI buttons have set their OnClick set in the inspector to `PositionCamera()` with id assigned to them.
- Run the scene and click on the buttons.

## BaseTarget view

![Image](https://github.com/mohitsethi32/PlayerCustomizationScreen/blob/master/Documentation/CustomizationCameraBase.png)

## head view

![Image](https://github.com/mohitsethi32/PlayerCustomizationScreen/blob/master/Documentation/CustomizationCameraFace.png)
