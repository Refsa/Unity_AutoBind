# AutoBind
Simple utility to auto bind components on scripts so you dont manually have to do it.

## Usage
Add the `AutoBindProxy` component to a GameObject to make it automatically bind that component. This isn't strictly necessary but plays more nicely with the Unity Runtime.

Put the `[AutoBind]` property in front of components in your script  
Ex:
```cs
public class SomeBehaviour : MonoBehaviour
{
    ...
    [AutoBind] Rigidbody rigidbody;
    ...
}
```  

Optionally you can ommit the `AutoBindProxy` component and manually call the bind function.
```cs
public class SomeBehaviour : MonoBehaviour
{
    [AutoBind] Rigidbody rigidbody;

    void Awake()
    {
        AutoBindComponent.AutoBind(this.gameObject);
    }
}
```  

You can also add the `[AutoBind]` attribute to classes that derive from MonoBehaviour, in which case you get access to the constructor with the `bindChildren` parameter. This will go down the hierarchy and bind all children.
```cs
[AutoBind(true)]
public class SomeBehaviour : MonoBehaviour
{
    ...
}
```  

## Other Usecases

You can also call `AutoBindComponent::AutoBind` directly on either a GameObject or a class that can reference a GameObject.  
Ex:
```cs
public class SomeClass
{
    [AutoBind] Rigidbody rigidbody;
}

public class SomeBehaviour : MonoBehaviour
{
    SomeClass someClass;
    void Awake()
    {
        someClass = new SomeClass();
        // This binds the Rigidbody on SomeClass from the current GameObject
        AutoBindComponent.AutoBind(someClass, this.gameObject);
    }
}
```