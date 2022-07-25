import { TestBed } from '@angular/core/testing';

import { ComicShelfService } from './comic-shelf.service';

describe('ComicShelfService', () => {
  let service: ComicShelfService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ComicShelfService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
