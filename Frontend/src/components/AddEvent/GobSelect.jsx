import React from 'react';
import Select, { createFilter, components } from 'react-select';

export class GobSelect extends React.Component {
  constructor(props) {
    super(props);
    let showedOptions = this.props.options;
  }
  render() {
    return (
      <>
        <Select
          placeholder={this.props.placeholder}
          options={
            this.props.options.length > 1500
              ? this.props.options.slice(0, 1500)
              : this.props.options
          }
          onChange={this.props.onChange}
          isSearchable={this.props.isSearchable}
          required={this.props.required}
          filterOption={createFilter({ ignoreAccents: false })}
          ignoreAccents="false"
          components={{ Option: CustomOption }}
        />
      </>
    );
  }
}

class CustomOption extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    const { innerProps, isFocused, ...otherProps } = this.props;
    const {
      onMouseMove,
      onMouseOver,
      ...otherInnerProps
    } = innerProps;
    const newProps = {
      innerProps: { ...otherInnerProps },
      ...otherProps,
    };
    return (
      <components.Option
        {...newProps}
        className="your-option-css-class"
      >
        {this.props.children}
      </components.Option>
    );
  }
}
