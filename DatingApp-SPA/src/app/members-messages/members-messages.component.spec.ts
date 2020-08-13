import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MembersMessagesComponent } from './members-messages.component';

describe('MembersMessagesComponent', () => {
  let component: MembersMessagesComponent;
  let fixture: ComponentFixture<MembersMessagesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MembersMessagesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MembersMessagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
