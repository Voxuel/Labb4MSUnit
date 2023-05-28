# Labb4MSUnit

## Unit tests

### Tests for Validating amount of money to transfer by:
Testing When given right amount should return true and continue.
Testing when given wrong amount or more than current users account balance should give false and return to start.

### Tests for login validation by:
Testing when given right username and password should return True.
Testing datarows of wrong inputs should return false.
Testing sending wrong datatypes should raise exception and not continue.
Testing when given right username but wrong password should return false with message saying wrong password.
Testing when the user is locked out after to many login attempts.

###Tests for creating new users as admin by:
Testing that after an user is created its added to the right datasource.
Testing when creating new user and giving unexpected currency type input.
Testing when given wrong datatype as input for new user should raise exception.
