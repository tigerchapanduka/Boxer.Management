export interface product{
    id:string;
    name:string;
    description:string;
    categoryId:string;
    userId:string;
    code:string;
    cost?:number;

}

export interface productList{
    errorMessage:string;
     products:Array<product>;
}