namespace Fools.Compilation.Recognizing:
  import MetaSharp.Transformation;

  grammar BasicFools < Parser:
    Main = i:Identifier -> true;
  end
end