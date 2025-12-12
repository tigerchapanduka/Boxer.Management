export interface category{
    id:string;
    name:string;
    errorMessage:string;
}

export interface categoryList{
    errorMessage:string;
     categories:Array<category>;
}