import { classNamesFunction, IStyle, IStyleFunctionOrObject, ITheme } from "@fluentui/react";
import { Result } from "axe-core";

interface IRuleResultProps {
    styles?: IStyleFunctionOrObject<IRuleResultStyleProps, IRuleResultStyles>;
    result: Result;
}

interface IRuleResultStyleProps {
    theme: ITheme;
}

interface IRuleResultStyles {
    ruleName?: IStyle;
}

const getClassNames = classNamesFunction<IRuleResultStyleProps, IRuleResultStyles>();

export { IRuleResultProps, IRuleResultStyleProps, IRuleResultStyles, getClassNames };