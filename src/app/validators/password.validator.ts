import { Validators } from '@angular/forms';

export class PasswordValidator {

  static readonly passwordValidators = [

    Validators.required,

    Validators.pattern(
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$/
    )

  ];

}