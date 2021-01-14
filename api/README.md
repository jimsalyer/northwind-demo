# Northwind Demo API

## Start

```shell-script
dotnet run
```

## Start in Watch Mode

```shell-script
dotnet watch run
```

## Access the API

Enter `http://localhost:5000/swagger` in your browser.

## Filtering and Sorting

### Query Parameters

- `sorts` is a comma-delimited ordered list of property names to sort by. Adding a - before the name switches to sorting descendingly.
- `filters` is a comma-delimited list of `{Name}{Operator}{Value}` where
  - `{Name}` is the name of a property with the Sieve attribute or the name of a custom filter method for TEntity
    You can also have multiple names (for OR logic) by enclosing them in brackets and using a pipe delimiter, eg. `(LikeCount|CommentCount)>10` asks if `LikeCount` or `CommentCount` is `>10`
  - `{Operator}` is one of the [Operators](#operators)
  - `{Value}` is the value to use for filtering
    You can also have multiple values (for OR logic) by using a pipe delimiter, eg. `Title@=new|hot` will return posts with titles that contain the text "new" or "hot"

### Operators

| Operator | Meaning                                      |
| -------- | -------------------------------------------- |
| `==`     | Equals                                       |
| `!=`     | Not equals                                   |
| `>`      | Greater than                                 |
| `<`      | Less than                                    |
| `>=`     | Greater than or equal to                     |
| `<=`     | Less than or equal to                        |
| `@=`     | Contains                                     |
| `_=`     | Starts with                                  |
| `!@=`    | Does not Contains                            |
| `!_=`    | Does not Starts with                         |
| `@=*`    | Case-insensitive string Contains             |
| `_=*`    | Case-insensitive string Starts with          |
| `==*`    | Case-insensitive string Equals               |
| `!=*`    | Case-insensitive string Not equals           |
| `!@=*`   | Case-insensitive string does not Contains    |
| `!_=*`   | Case-insensitive string does not Starts with |
