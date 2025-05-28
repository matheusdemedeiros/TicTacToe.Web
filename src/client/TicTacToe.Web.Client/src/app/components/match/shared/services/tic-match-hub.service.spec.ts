import { TestBed } from '@angular/core/testing';

import { TicMatchHubService } from './tic-match-hub.service';

describe('TicMatchHubService', () => {
  let service: TicMatchHubService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TicMatchHubService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
