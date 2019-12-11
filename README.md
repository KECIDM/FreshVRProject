# FreshVRProject

This is an ongoing conversion of an old iOS Cardboard VR project for the Oculus Go. It is not a finished product.

Directory MyScene/TestScene3 is the most complete.
The OVRCameraRig has a Weapon object as a child of the RightControllerAnchor.

The WeaponActions Script attached to it works with PlayerEvents.cs, Pointer.cs, Reticle.cs and VRTeleporter.cs to allow the user to
select different VR capabilities, represented by different prefabs, the default tool is a teleporter, the tablet is for interacting
with doors and interfaces, the 2 guns are weapons, the hand is a distance grabber.

The little 'scorpion' robots take health away rapidly by lasering you.
