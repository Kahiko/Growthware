import { Component, OnInit } from '@angular/core';
import {NestedTreeControl} from '@angular/cdk/tree';
import {MatTreeNestedDataSource} from '@angular/material/tree';
// Feature
import { FileManagerService } from '../../file-manager.service';
import { IDirectoryTree } from '../../directory-tree.model';

@Component({
  selector: 'gw-lib-directory-tree',
  templateUrl: './directory-tree.component.html',
  styleUrls: ['./directory-tree.component.scss']
})
export class DirectoryTreeComponent implements OnInit {

  activeNode?: IDirectoryTree;
  treeControl = new NestedTreeControl<IDirectoryTree>(node => node.children);
  dataSource = new MatTreeNestedDataSource<IDirectoryTree>();

  constructor(private _FileManagerSvc: FileManagerService) { 
    this._FileManagerSvc.getDirectories(1).then((response) => {
      // console.log(response);
      this.dataSource.data = response;
    }).catch((error) => {

    });
  }

  ngOnInit(): void {
  }
  hasChild = (_: number, node: IDirectoryTree) => !!node.children && node.children.length > 0;

  selectDirectory(node: any): void {
    // alert('hi from selectSelectedDirectory');
    this.activeNode = node;
  }
}
