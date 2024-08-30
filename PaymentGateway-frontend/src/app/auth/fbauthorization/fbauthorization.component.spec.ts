import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FbauthorizationComponent } from './fbauthorization.component';

describe('FbauthorizationComponent', () => {
  let component: FbauthorizationComponent;
  let fixture: ComponentFixture<FbauthorizationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FbauthorizationComponent]
    });
    fixture = TestBed.createComponent(FbauthorizationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
