Twitter threads on better CLI
https://twitter.com/quorralyne/status/1225414312950403072?s=20


## Basic goals

We have two orthogonal expectations: 

* Model rules: People will want to define their own rules for app models 
* Model extraction: We will support multiple ways to describe models - reflection, JSON, Roslyn/source generation

We expect people will want the app models they build to apply as broadly as practical across different ways to describe models .

Also, the changes to rules may be quite simple - removing a rule or using a different attribute name - so there needs to not be a cliff of our rules or tons of work. An unruly combination of rules and extraction mechanism coudl be a mess. 

To address this, the problem a few known commonalities and examples (this list will likely extend):

* Attribute (Attributes in Reflection and Roslyn, child values in JSON, not used in OptDoc)
  * Bool (presence of attribute is generally sufficient for true)
  * String or other native type
  * Custom type
* Strings (grabbing meaning from names, happily this is universal. )

A strategy is a single type of rule like: determining if a name has the "Arg" suffix
Strategies (might rename to StrategySet) is a collection of strategies and the logic for combining them. The logic differs by rule type, but includes

* First or default
* First or null if not found (bool, we may use nullable)
* True if any
* All

Most of the things strategies are needed for require a strategy. Defaults are not optional, but overruling them is essential. This may involve a flag as to whether non-defaults were set. 

People need to understand strageies rules. They need to self describe. This is started with the Description abstract property on Strategy. We also need strategy on Strategies to include the logic and a grouping Description that describes the purpose of each (IsArgument vs. SubCommands vs Description) 

People may need additional tools to debug why their model was built the way it was. Once we have descriptions, we can see if this is a problem. (This would be a different user, one debugging JSON or reflection input, not the rules themselves.)

## Examples

Description rules: 
- "description" string attribute value (this has special interpretation for XML Docs)

IsArgument rules:
- Name ends in "Arg" or "Argument"
- "argument" bool attribute 

Arity rules:
- "arity" attribute custom arity type

IsCommand rules:
- "command" bool attribute
- Name ends in "Cmd"
- ComplexType strategy

Name rules:
- "name" string attribute
- "option" or "arg" attribute Name property if present
- item name

IsRequired rules:
- "required" bool attribute
- Option: "option" attribute OptionRequired property
- Argument: "option" attribute ArgumentRequired property
- "argument" attribute Required property

## Source Generation

Strategies for source gen and reflection should be defined once
for the model author. IOW, you should be able to say "NameHasSuffix".

Attributes:

This probably means that the attribute strategy is a list of 
type/attribute prop name pairs. 

This would be pretty ugly for reflection because things that are 
now Func would become reflection that might make assumptions. 

Better bet might be to use expressions and rip them apart for 
the source gen strategy. In this case, the strategy should probably 
be a (Type, Func<Attribute, T>) where T is an extract Expression. 
It should work for Bool, string and arity. 

This would change Attribute

String: 