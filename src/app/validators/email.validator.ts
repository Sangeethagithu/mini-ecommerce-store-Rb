import { Validators } from '@angular/forms';

export class EmailValidator {

  static readonly emailValidators = [

    Validators.required,

    Validators.pattern(
      /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    )

  ];

}